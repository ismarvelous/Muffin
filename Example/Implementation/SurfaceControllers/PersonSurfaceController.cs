using System.Web.Mvc;
using Example.Implementation.Models;
using Muffin.Controllers;
using Muffin.Core;
using Umbraco.Web.Mvc;

namespace Example.Implementation.SurfaceControllers
{
    //Example of how to your own controller for a partial..
    public class PersonSurfaceController : BaseSurfaceController
    {

        public PersonSurfaceController(ISiteRepository rep)
            : base(rep)
        {

        }

        [ChildActionOnly]
        public ActionResult GetPerson(int contentId)
        {
            var content = Repository.FindById(contentId) as PersonItem;
            return PartialView("Person", content);
            return null;
        }
    }
}