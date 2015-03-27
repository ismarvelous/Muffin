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
using Our.Umbraco.Ditto;
using Umbraco.Web.Models;

namespace Muffin.Controllers
{
    /// <summary>
    /// Default Search Controller..
    /// </summary>
    public abstract class SearchBaseController : BaseController
    {
        protected SearchBaseController(ISiteRepository rep, IMapper map)
			: base (rep, map)
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
            var @base = model.Content as ModelBase;
            IModel content = @base ?? new ModelBase(model.Content);

            var resultModel = new SearchModel(content,
                q); //this.Request.QueryString["q"]);

            //late binding for pagedresults
            resultModel.PagedResults = () => resultModel.Container.Skip(s*(p - 1))
                .Take(s); 

			resultModel.CurrentPage = p;
			resultModel.PageSize = s;

            return View(resultModel);
        }
    }
}