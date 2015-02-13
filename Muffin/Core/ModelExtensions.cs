using System.Collections.Generic;
using System.Web.Mvc;
using Muffin.Core.Models;
using Umbraco.Core.Models;

namespace Muffin.Core
{
    public static class ModelExtensions
    {
        private static IMapper Mapper
        {
            get
            {
                return DependencyResolver.Current.GetService<IMapper>();
            }
        }

        public static MvcHtmlString AsJson(this IPublishedContent content, string[] properties = null, bool includeHiddenItems = true)
        {
            return Mapper.AsJson(content, properties, includeHiddenItems);
        }

        public static MvcHtmlString AsJson(this IEnumerable<IPublishedContent> collection, string[] properties = null,
            bool includeHiddenItems = true)
        {
            return Mapper.AsJson(collection, properties, includeHiddenItems);
        }

        /// <summary>
        /// Overrule Umbraco's AsDynamic
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static dynamic AsDynamic(this IModel model)
        {
            return Mapper.AsDynamicIModel(model);
        }

        /// <summary>
        /// Overrule Umbraco's AsDynamic
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static dynamic AsDynamic(this IEnumerable<IModel> model)
        {
            return Mapper.AsDynamicIModel(model);
        }
    }
}
