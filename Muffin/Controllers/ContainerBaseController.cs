using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Castle.DynamicProxy;
using Muffin.Core;
using Muffin.Core.Models;
using Muffin.Core.ViewModels;
using Umbraco.Web.Models;

namespace Muffin.Controllers
{
    /// <summary>
	/// Default Container Controller..
    /// </summary>
	public abstract class ContainerBaseController : BaseController
    {
        protected ContainerBaseController(ISiteRepository rep)
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
            var content = model.Content as IModel;

            var result = new CollectionContentViewModel<IModel>(content, content);

			result.PagedResults = () => result.Container
			    .Skip(s * (p - 1))
			    .Take(s);
             
			result.CurrentPage = p;
			result.PageSize = s;

			return View(result);
		}
    }
}