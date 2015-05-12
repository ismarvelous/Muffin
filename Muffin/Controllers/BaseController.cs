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
using Muffin.Infrastructure;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;

namespace Muffin.Controllers
{
    /// <summary>
    /// Default base controller for Umbraco projects.
    /// More about Route Hijacking: http://our.umbraco.org/documentation/Reference/Mvc/custom-controllers
    /// </summary>
    public class BaseController : Controller, IRenderMvcController
        // more info: http://our.umbraco.org/documentation/Reference/Mvc/custom-controllers
    {

        protected ISiteRepository Repository;
        protected IMapper Mapper;

        public BaseController(ISiteRepository rep, IMapper mapper)
            : base()
        {
            Repository = rep;
            Mapper = mapper;
        }

        [DonutOutputCache(Duration = 86400, VaryByCustom = "url", Options = OutputCacheOptions.NoCacheLookupForPosts)]
        public virtual ActionResult Index(RenderModel model) //Template name, default is Index
        {
            if (model.Content is IModel) //Content is ModelBase or any other IModel created by the muffin framework
            {
                return View(model.Content);
            }

            //if there is not a muffin IModel version found, create a default modelbase and wrap it as a dynamic object.
            return View(Mapper.AsDynamicIModel(new ModelBase(model.Content)));
        }

        //http://our.umbraco.org/documentation/Reference/Mvc/custom-controllers
        //https://github.com/Shandem/Umbraco4Docs/blob/4.8.0/Documentation/Reference/Mvc/surface-controllers.md
        //http://our.umbraco.org/forum/developers/api-questions/38048-Umbraco-411-MVC-Custom-Routing-Content-is-null-How-can-I-load-content?p=1


        public virtual ActionResult Rss(string path)
        {
            //var path = Request.Url.GetAbsolutePathDecoded().ToLower();
            //var contentPath = path.Remove(path.IndexOf("json", StringComparison.InvariantCulture)); 
            var content = Repository.FindByUrl<ModelBase>(string.Format("/{0}", path));
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
            var content = Repository.FindByUrl<ModelBase>(string.Format("/{0}", path));
            return new ContentResult()
            {
                Content =  Mapper.AsJson(content).ToString(),
                ContentType = "application/json"
            };
        }
}
}