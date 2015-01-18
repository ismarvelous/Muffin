using System.Linq;
using System.Web.Mvc;
using Muffin.Core;
using Muffin.Core.Models;
using Our.Umbraco.Ditto;
using Umbraco.Web.Models;

namespace Muffin.Controllers
{
    /// <summary>
	/// Default Container Controller..
    /// </summary>
	public abstract class ContainerBaseController : BaseController
    {
        protected ContainerBaseController(ISiteRepository rep, IMapper map)
			: base (rep, map)
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
            var @base = model.Content as ModelBase;
            IModel content = @base ?? new ModelBase(model.Content);

			var resultModel = new CollectionModel(content, model.Content.Children);

			resultModel.PagedResults = () => resultModel.Container //or model.Content.Children which is the same in this case.
			    .Skip(s * (p - 1))
			    .Take(s);
             
			resultModel.CurrentPage = p;
			resultModel.PageSize = s;

			return View(resultModel);
		}
    }
}