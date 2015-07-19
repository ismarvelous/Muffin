using Muffin.Core;
using Umbraco.Web.Mvc;

namespace Muffin.Controllers
{
    public class BaseSurfaceController : SurfaceController
    {
        protected ISiteRepository Repository { get; set; }

        public BaseSurfaceController(ISiteRepository repository)
        {
            Repository = repository;
        }
    }
}
