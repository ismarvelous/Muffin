using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Muffin.Core;
using Muffin.Core.Models;
using Muffin.Infrastructure.Models;
using Umbraco.Core;
using Umbraco.Core.Models;

namespace Muffin.Infrastructure
{
    public class Mapper: IMapper 
    {
        private static dynamic AsExpando(IPublishedContent content, string[] aliases)
        {
            IDictionary<string, object> expando = new ExpandoObject();

            if (aliases != null)
            {
                foreach (var prop in content.Properties.Where(p => aliases.Contains(p.PropertyTypeAlias)))
                {
                    try
                    {
                        if (prop.Value is HtmlString || prop.Value is MediaModel)
                            //htmlstring & DynamicMediaModel can not be serialized with Newtonsoft json..
                            expando.Add(prop.PropertyTypeAlias, prop.Value.ToString());
                        else
                            expando.Add(prop.PropertyTypeAlias, prop.Value);
                    }
                    catch
                    {
                        // ignored, some values need a PublishedContentRequest which we don't have here. these values are not supported
                    }
                }
            }
            else
            {
                foreach (var prop in content.Properties)
                {
                    try
                    {
                        if (prop.Value is HtmlString || prop.Value is MediaModel) //htmlstring & DynamicMediaModel can not be serialized with Newtonsoft json..
                            expando.Add(prop.PropertyTypeAlias, prop.Value.ToString());
                        else
                            expando.Add(prop.PropertyTypeAlias, prop.Value);
                    }
                    catch
                    {
                        // ignored, some values need a PublishedContentRequest which we don't have here. these values are not supported
                    }
                }
            }

            return (ExpandoObject) expando;
        }

        private static IEnumerable<dynamic> AsExpando(IEnumerable<IPublishedContent> collection, string[] aliases)
        {
            return collection.Select(item => AsExpando(item, aliases));
        }

        public MvcHtmlString AsJson(object obj)
        {
            var content = obj as IPublishedContent;
            if (content != null)
            {
                return AsJson(content);
            }

            var contents = obj as IEnumerable<IPublishedContent>;
            if (contents != null)
            {
                return AsJson(contents);
            }

            //default
            var ret = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            return MvcHtmlString.Create(ret);
        }

        public MvcHtmlString AsJson(IPublishedContent content, string[] properties = null, bool includeHiddenItems = true)
        {
            var ret = string.Empty;
            if (!(bool)content.GetProperty(Constants.Conventions.Content.NaviHide).Value || includeHiddenItems)
            {
                ret = Newtonsoft.Json.JsonConvert.SerializeObject(AsExpando(content, properties));
            }

            return MvcHtmlString.Create(ret);
        }

        public MvcHtmlString AsJson(IEnumerable<IPublishedContent> collection, string[] properties = null, bool includeHiddenItems = true)
        {
            var ret = "undefined";
            ret = Newtonsoft.Json.JsonConvert.SerializeObject(!includeHiddenItems ? 
                AsExpando(collection.Where(p => !(bool)p.GetProperty(Constants.Conventions.Content.NaviHide).Value), properties) : 
                AsExpando(collection, properties));

            return MvcHtmlString.Create(ret);
        }

        public IModel AsDynamicIModel(IModel model)
        {
            var wrapper = model as DynamicModelBaseWrapper;
            return wrapper ?? new DynamicModelBaseWrapper(model);
        }

        public IEnumerable<IModel> AsDynamicIModel (IEnumerable<IModel> model)
        {
            return model.Select(AsDynamicIModel);
        }
    }
}