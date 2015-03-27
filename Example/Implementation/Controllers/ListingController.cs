using Muffin.Controllers;
using Muffin.Core;

namespace Example.Implementation.Controllers
{
	public class ListingController : ContainerBaseController
	{
		public ListingController(ISiteRepository rep, IMapper map)
			: base (rep, map)
        {
        }

		public override System.Web.Mvc.ActionResult Index(Umbraco.Web.Models.RenderModel model)
		{
			return Container(model, 1, 10);
		}

		public System.Web.Mvc.ActionResult Listing(Umbraco.Web.Models.RenderModel model, int p = 1, int s = 10)
		{
			return Container(model, p, s);
		}
	}
}