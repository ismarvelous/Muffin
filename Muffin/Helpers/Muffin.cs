using Muffin.Core.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Umbraco.Core;
using Umbraco.Core.Models;

namespace Muffin.Helpers
{
    public static class Muffin
    {
        public static MvcHtmlString ToJson(object obj)
        {
            if (obj is IPublishedContent)
            {
                return ToJson((IPublishedContent)obj);
            }
            else if (obj is IEnumerable<IPublishedContent>)
            {
                return ToJson((IEnumerable<IPublishedContent>)obj);
            }
            else
            {
                var ret = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                return MvcHtmlString.Create(ret);
            }
        }

        public static MvcHtmlString ToJson(IPublishedContent content, string[] properties = null, bool includeHiddenItems = true)
        {
            var ret = string.Empty;
            if (!(bool)content.GetProperty(Constants.Conventions.Content.NaviHide).Value || includeHiddenItems)
            {
                ret = Newtonsoft.Json.JsonConvert.SerializeObject(ToDynamic(content, properties));
            }

            return MvcHtmlString.Create(ret);
        }

        public static MvcHtmlString ToJson(IEnumerable<IPublishedContent> collection, string[] properties = null, bool includeHiddenItems = true)
        {
            var ret = "undefined";
            if (!includeHiddenItems) //not using IsVisible() here because it's not easy to mock for testing...
                ret = Newtonsoft.Json.JsonConvert.SerializeObject(ToDynamic(collection.Where(p => !(bool)p.GetProperty(Constants.Conventions.Content.NaviHide).Value), properties));
            else
                ret = Newtonsoft.Json.JsonConvert.SerializeObject(ToDynamic(collection, properties));

            return MvcHtmlString.Create(ret);
        }

        internal static dynamic ToDynamic(IPublishedContent content, string[] aliases)
        {
            IDictionary<string, object> expando = new ExpandoObject();

            if (aliases != null)
            {
                foreach (var prop in content.Properties.Where(p => aliases.Contains(p.PropertyTypeAlias)))
                {
                    if (prop.Value is HtmlString || prop.Value is DynamicMediaModel) //htmlstring & DynamicMediaModel can not be serialized with Newtonsoft json..
                        expando.Add(prop.PropertyTypeAlias, prop.Value.ToString());
                    else
                        expando.Add(prop.PropertyTypeAlias, prop.Value);
                }
            }
            else
            {
                foreach (var prop in content.Properties)
                {
                    if (prop.Value is HtmlString || prop.Value is DynamicMediaModel) //htmlstring & DynamicMediaModel can not be serialized with Newtonsoft json..
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
    }
}
