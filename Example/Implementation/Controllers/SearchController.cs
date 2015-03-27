using System.Web.Mvc;
using Muffin.Controllers;
using Muffin.Core;
using Umbraco.Web.Models;

namespace Example.Implementation.Controllers
{
    /// <summary>
    /// Controller for search..
    /// </summary>
    public class SearchController : SearchBaseController
    {
		public SearchController(ISiteRepository rep, IMapper map)
			: base (rep, map)
        {
        }

        public override ActionResult Index(RenderModel model) //default Umbraco route
        {
            return Search(model, 1, 10, "");
        }

		public override ActionResult Search(RenderModel model, int p = 1, int s = 10, string q = "")
		{
			return base.Search(model, p, s, q);
		}
    }
}