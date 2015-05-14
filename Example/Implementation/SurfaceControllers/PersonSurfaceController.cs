using System.Web.Mvc;
using Example.Implementation.ViewModels;
using Muffin.Core;
using Umbraco.Web.Mvc;

namespace Example.Implementation.SurfaceControllers
{
    //Example of how to your own controller for a partial..
    public class PersonSurfaceController : SurfaceController
    {
        protected ISiteRepository Repository;

        public PersonSurfaceController(ISiteRepository rep)
            : base()
        {
            Repository = rep;
        }

        [ChildActionOnly]
        public ActionResult GetPerson(int contentId)
        {
            //call your business services or call your repositories..
            dynamic content = Repository.FindById(contentId);

            //map the documenttype to a typed object..
            return PartialView("Person", new Person
            {
                Name = content.Name,
                City = content.Woonplaats
            });
        }
    }
}