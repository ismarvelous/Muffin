using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Muffin.Core.Models;
using Umbraco.Core.Models;

namespace Muffin.Core
{
    public interface IMapper
    {
        MvcHtmlString AsJson(object obj);
        MvcHtmlString AsJson(IPublishedContent content, string[] properties = null, bool includeHiddenItems = true);
        MvcHtmlString AsJson(IEnumerable<IPublishedContent> collection, string[] properties = null, bool includeHiddenItems = true);

        IModel AsDynamicIModel(IModel model);
    }
}
