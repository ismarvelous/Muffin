using System.Collections.Generic;
using Microsoft.Web.Mvc;
using Umbraco.Core;
using Muffin.Core;

namespace Muffin.Mvc
{
    public class ThemeViewEngine : FixedRazorViewEngine
    {
        private readonly IEnumerable<string> _viewLocations = new[] { "/{0}.cshtml" };
        private readonly IEnumerable<string> _umbracoPartialViewLocations = new[] { "/Partials/{0}.cshtml", "/MacroPartials/{0}.cshtml", "/{0}.cshtml" };

        public ThemeViewEngine()
        {
            var themefolder = string.Format("~/Themes/{0}/Views", Settings.CurrentTheme);

            ViewLocationFormats = _viewLocations.ForEach(l => string.Concat(themefolder, l));
            PartialViewLocationFormats = _umbracoPartialViewLocations.ForEach(l => string.Concat(themefolder, l));

            AreaPartialViewLocationFormats = new string[] { };
            AreaViewLocationFormats = new string[] { };
        }

        //TODO: find view / find partial view

    }
}
