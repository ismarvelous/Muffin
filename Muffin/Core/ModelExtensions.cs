using System.Collections.Generic;
using System.Web.Mvc;
using Muffin.Core.Models;
using Umbraco.Core.Models;

namespace Muffin.Core
{
    public static class ModelExtensions
    {
        private static IMapper Mapper => DependencyResolver.Current.GetService<IMapper>();

        public static MvcHtmlString AsJson(this IPublishedContent content, string[] properties = null, bool includeHiddenItems = true)
        {
            return Mapper.AsJson(content, properties, includeHiddenItems);
        }

        public static MvcHtmlString AsJson(this IEnumerable<IPublishedContent> collection, string[] properties = null,
            bool includeHiddenItems = true)
        {
            return Mapper.AsJson(collection, properties, includeHiddenItems);
        }
    }
}
