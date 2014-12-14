﻿using System.Linq;
using System.Web.Mvc;
using Muffin.Core;
using Muffin.Core.Models;
using Umbraco.Web.Models;

namespace Muffin.Controllers
{
    /// <summary>
	/// Default Container Controller..
    /// </summary>
	public abstract class ContainerBaseController : DynamicBaseController
    {
		public ContainerBaseController(ISiteRepository rep)
			: base (rep)
        {
        }

		public override abstract ActionResult Index(RenderModel model);

        /// <summary>
        /// This default implementation is using the childcollection as the container..
        /// Allowed querystring parameters:
        /// p = Pagenumber
        /// s = pageSize
        /// </summary>
        /// <param name="model"></param>
        /// <param name="p"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public virtual ActionResult Container(RenderModel model,
			int p=1, //read querystring parameters without using this.Request.
            int s=10)
		{
			var resultModel = new DynamicCollectionModel(model.Content.As<ModelBase>(), model.Content.Children);

			resultModel.PagedResults = () => resultModel.Container //or model.Content.Children which is the same in this case.
			    .Skip(s * (p - 1))
			    .Take(s)
                .Select(n => new DynamicModelBaseWrapper(n)); //todo: bad code!!!! this conversion is here because we need to use it as a dynamic
             
			resultModel.CurrentPage = p;
			resultModel.PageSize = s;

			return View(resultModel);
		}
    }
}