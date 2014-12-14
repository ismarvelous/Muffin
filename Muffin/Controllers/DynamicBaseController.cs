using System;
using System.Linq;
using System.Web.Mvc;
using Muffin.Core;
using Muffin.Core.Models;
using Muffin.Mvc;
using Umbraco.Core;
using Umbraco.Web;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;
using DevTrends.MvcDonutCaching;
using Umbraco.Core.Models;

namespace Muffin.Controllers
{
    /// <summary>
    /// Default base controller for Umbraco projects.
    /// More about Route Hijacking: http://our.umbraco.org/documentation/Reference/Mvc/custom-controllers
    /// </summary>
    public class DynamicBaseController : Controller, IRenderMvcController
        // more info: http://our.umbraco.org/documentation/Reference/Mvc/custom-controllers
    {
        protected ISiteRepository Repository;

        public DynamicBaseController(ISiteRepository rep)
            : base()
        {
            Repository = rep;
        }

        [DonutOutputCache(Duration = 86400, VaryByCustom = "url", Options = OutputCacheOptions.NoCacheLookupForPosts)]
        public virtual ActionResult Index(RenderModel model) //Template name, default is Index
        {
            if (model.Content is ModelBase)
            {
                return View(model.Content);
            }

            //if there is not a typed version found, wrap the model in a dynamic class..
            return View(new DynamicModelBaseWrapper(new ModelBase(model.Content)));
        }

        //http://our.umbraco.org/documentation/Reference/Mvc/custom-controllers
        //https://github.com/Shandem/Umbraco4Docs/blob/4.8.0/Documentation/Reference/Mvc/surface-controllers.md
        //http://our.umbraco.org/forum/developers/api-questions/38048-Umbraco-411-MVC-Custom-Routing-Content-is-null-How-can-I-load-content?p=1


        public virtual ActionResult Rss(string path)
        {
            //var path = Request.Url.GetAbsolutePathDecoded().ToLower();
            //var contentPath = path.Remove(path.IndexOf("json", StringComparison.InvariantCulture)); 
            var content = Repository.FindByUrl(string.Format("/{0}", path));
            return new RssActionResult(content);
        }

        public virtual ActionResult Sitemap()
        {
            return new SitemapActionResult(Repository);
        }

        public virtual ActionResult Json(string path)
        {
            //var path = Request.Url.GetAbsolutePathDecoded().ToLower();
            //var contentPath = path.Remove(path.IndexOf("json", StringComparison.InvariantCulture)); 
            var content = Repository.FindByUrl(string.Format("/{0}", path));
            return new ContentResult()
            {
                Content =  content.AsJson().ToString(),
                ContentType = "application/json"
            };
        }
}
}