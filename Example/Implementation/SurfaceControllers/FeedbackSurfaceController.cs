using System.Web.Mvc;
using Example.Implementation.Models;
using Muffin.Controllers;
using Muffin.Core;
using Umbraco.Web.Mvc;

namespace Example.Implementation.SurfaceControllers
{
    //Example of how to your own controller for a partial..
    public class FeedbackSurfaceController : BaseSurfaceController
    {
        public FeedbackSurfaceController(ISiteRepository rep)
            : base(rep)
        {

        }

        [HttpGet]
        public ActionResult Feedback()
        {

            return PartialView("Feedback");
        }

        [HttpPost]
        public ActionResult Feedback(string feedback)
        {
            var current = Repository.GetObject<Feedback>(CurrentPage.Id);
            return PartialView("Feedback", string.Format("Bedankt voor uw feedback sore yes: {0} no {1}", current.TotalYes, current.TotalNo));
        }
        
        [HttpPost]
        public ActionResult Save(string feedback)
        {
            var current = Repository.GetObject<Feedback>(CurrentPage.Id);
            current.ContentId = CurrentPage.Id;
            current.TotalNo = current.TotalNo + (feedback != "yes" ? 1 : 0);
            current.TotalYes = current.TotalYes + (feedback == "yes" ? 1 : 0);
            current.Url = CurrentPage.Url;

            Repository.SaveObject(current);

            return CurrentUmbracoPage();
        }
    }
}