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
using Muffin.Core.ViewModels;
using Muffin.Infrastructure;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.Persistence.Migrations.Syntax.Update;

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

        public BaseController(ISiteRepository rep)
            : base()
        {
            Repository = rep;
        }

        [DonutOutputCache(Duration = 86400, VaryByCustom = "url", Options = OutputCacheOptions.NoCacheLookupForPosts)]
        public virtual ActionResult Index(RenderModel model) //Template name, default is Index
        {
            var content = model.Content as IModel;
            return View(content != null ? 
                CreateViewModel(content) : 
                CreateViewModel((new ModelBase(model.Content)).AsDynamic()));

            //if there is not a muffin IModel version found, create a default modelbase and wrap it as a dynamic object.
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
                Content =  content.AsJson().ToString(),
                ContentType = "application/json"
            };
        }

        /// <summary>
        /// helper function to create a default ContentViewModel.T.
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public virtual IContentViewModel<T> CreateViewModel<T>(T content)
            where T : IModel
        {
            var type = typeof(ContentViewModel<>).MakeGenericType(content.GetType());
            return Activator.CreateInstance(type, content) as IContentViewModel<T>;
        }
    }
}