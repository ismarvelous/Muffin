using System.Linq;
using Muffin.Controllers;
using Muffin.Core;
using Muffin.Core.Models;
using Muffin.Core.ViewModels;

namespace Example.Implementation.Controllers
{
	public class ListingController : ContainerBaseController
	{
		public ListingController(ISiteRepository rep)
			: base (rep)
        {
        }

		public override System.Web.Mvc.ActionResult Index(Umbraco.Web.Models.RenderModel model)
		{
			return Listing(model, 1, 10);
		}

		public System.Web.Mvc.ActionResult Listing(Umbraco.Web.Models.RenderModel model, int p = 1, int s = 10)
		{
            var content = model.Content as Models.Base;

            var result = new CollectionContentViewModel<Models.Base>(content, content);

            result.PagedResults = () => result.Container
                .Skip(s * (p - 1))
                .Take(s);

            result.CurrentPage = p;
            result.PageSize = s;

            return View(result);
        }
	}
}