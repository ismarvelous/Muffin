using Examine;
using Examine.LuceneEngine.Providers;
using Muffin.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Muffin.Core;
using Muffin.Core.Models;
using Muffin.Core.ViewModels;
using Umbraco.Web.Models;

namespace Muffin.Controllers
{
    /// <summary>
    /// Default Search Controller..
    /// </summary>
    public abstract class SearchBaseController : BaseController
    {
        protected SearchBaseController(ISiteRepository rep)
			: base (rep)
        {
        }

        public override abstract ActionResult Index(RenderModel model);

        /// <summary>
        /// Search PublishedContent using the default searchprovider
        /// Allowed querystring parameters:
        /// p = Pagenumber
        /// s = pageSize
        /// q = Query
        /// </summary>
        /// <param name="model"></param>
        /// <param name="p"></param>
        /// <param name="s"></param>
        /// <param name="q"></param>
        /// <returns></returns>
        public virtual ActionResult Search(RenderModel model, 
            int p=1, //read querystring parameters without using this.Request.
            int s=10,
            string q="") //for search template
        {
            var type = typeof(SearchContentViewModel<>).MakeGenericType(model.Content.GetType());
            var result = Activator.CreateInstance(type, model.Content, q) as ISearchContentViewModel<IModel>;

            //late binding for pagedresults
            result.PagedResults = () => result.Container.Skip(s*(p - 1))
                .Take(s); 

			result.CurrentPage = p;
			result.PageSize = s;

            return View(result);
        }
    }
}