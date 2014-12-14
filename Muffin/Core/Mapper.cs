using System;
using Muffin.Core.Models;
using umbraco;
using Umbraco.Core;
using Umbraco.Core.Models;
using System.Web.Mvc;
using System.Reflection; //for service locator.
using Umbraco.Web;
using System.Collections.Generic;
using System.Dynamic;
using System.Web;
using System.Linq;


namespace Muffin.Core
{
    public static class Mapper
    {
		/// <summary>
		/// Turn your IPublishedContent model into your own typed version.
		/// Supported are models with a parameterless constructor and DynamicModel ViewModels.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="content"></param>
		/// <returns></returns>
		public static T As<T>(this IPublishedContent content) //todo: try to use Ditto instead of this custom implementation!
		{
		    if (content is T) //besure you don't convert to the same type!
		    {
		        return (T)content;
		        //todo: throw new ArgumentException(
		        //    String.Format(
		        //        "Muffin.Core.Mapper argument exception; Now mapping needed! You try to convert a type <{0}> into it's own type",
		        //        typeof (T).FullName));
		    }

		    T obj;
		    if (typeof (T).Inherits<DynamicModelBaseWrapper>())
		    {
		        var source = content as ModelBase ?? new ModelBase(content);
		        return (T) Activator.CreateInstance(typeof (T), source);
		    }

            //typed!
            if (typeof (T).Inherits<ModelBase>())
            {
                obj = (T) Activator.CreateInstance(typeof (T), content);
            }
            else
            {
                //if you don't use the Muffin, you only need this region, to create a simple IPublishContent -> ViewModel mapper.
                obj = Activator.CreateInstance<T>();
            }

		    foreach (var prop in typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty))
		    {
		        if (content.HasProperty(prop.Name))
		        {
		            prop.SetValue(obj, content.GetPropertyValue(prop.Name), null);
		        }
                else if (content.HasProperty(char.ToLower(prop.Name[0]) + prop.Name.Substring(1))) //lowercase starting character for alias
                {
                    prop.SetValue(obj, content.GetPropertyValue(char.ToLower(prop.Name[0]) + prop.Name.Substring(1)), null);
                }
                else
                {
                    var sourceProp = content.GetType().GetProperty(prop.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty);
                    if (sourceProp != null && sourceProp.CanWrite && sourceProp.GetSetMethod(true).IsPublic)
                    {
                        prop.SetValue(obj, sourceProp.GetValue(content), null);
                    }
                }
		    }

		    return obj;
            
		}

        public static IEnumerable<T> As<T>(this IEnumerable<IPublishedContent> items)
        {
            return items.ForEach(i => i.As<T>());
        }

        internal static dynamic ToDynamic(IPublishedContent content, string[] aliases)
        {
            IDictionary<string, object> expando = new ExpandoObject();

            if (aliases != null)
            {
                foreach (var prop in content.Properties.Where(p => aliases.Contains(p.PropertyTypeAlias)))
                {
                    if (prop.Value is HtmlString || prop.Value is MediaModel) //htmlstring & DynamicMediaModel can not be serialized with Newtonsoft json..
                        expando.Add(prop.PropertyTypeAlias, prop.Value.ToString());
                    else
                        expando.Add(prop.PropertyTypeAlias, prop.Value);
                }
            }
            else
            {
                foreach (var prop in content.Properties)
                {
                    if (prop.Value is HtmlString || prop.Value is MediaModel) //htmlstring & DynamicMediaModel can not be serialized with Newtonsoft json..
                        expando.Add(prop.PropertyTypeAlias, prop.Value.ToString());
                    else
                        expando.Add(prop.PropertyTypeAlias, prop.Value);
                }
            }

            return expando as ExpandoObject;
        }

        internal static IEnumerable<dynamic> ToDynamic(IEnumerable<IPublishedContent> collection, string[] aliases)
        {
            return collection.Select(item => ToDynamic(item, aliases));
        }

        public static MvcHtmlString AsJson(object obj)
        {
            var content = obj as IPublishedContent;
            if (content != null)
            {
                return content.AsJson();
            }

            var contents = obj as IEnumerable<IPublishedContent>;
            if (contents != null)
            {
                return contents.AsJson();
            }

            //default
            var ret = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            return MvcHtmlString.Create(ret);
        }

        public static MvcHtmlString AsJson(this IPublishedContent content, string[] properties = null, bool includeHiddenItems = true)
        {
            var ret = string.Empty;
            if (!(bool)content.GetProperty(Constants.Conventions.Content.NaviHide).Value || includeHiddenItems)
            {
                ret = Newtonsoft.Json.JsonConvert.SerializeObject(ToDynamic(content, properties));
            }

            return MvcHtmlString.Create(ret);
        }

        public static MvcHtmlString AsJson(this IEnumerable<IPublishedContent> collection, string[] properties = null, bool includeHiddenItems = true)
        {
            var ret = "undefined";
            if (!includeHiddenItems) //not using IsVisible() here because it's not easy to mock for testing...
                ret = Newtonsoft.Json.JsonConvert.SerializeObject(ToDynamic(collection.Where(p => !(bool)p.GetProperty(Constants.Conventions.Content.NaviHide).Value), properties));
            else
                ret = Newtonsoft.Json.JsonConvert.SerializeObject(ToDynamic(collection, properties));

            return MvcHtmlString.Create(ret);
        }
    }
}