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
            ViewLocationFormats = _viewLocations.ForEach(l => string.Concat(Settings.CurrentThemeViewPath, l));
            PartialViewLocationFormats = _umbracoPartialViewLocations.ForEach(l => string.Concat(Settings.CurrentThemeViewPath, l));

            AreaPartialViewLocationFormats = new string[] { };
            AreaViewLocationFormats = new string[] { };
        }

        //2014-12-08 the themeview engine is underconstruction
        //TODO: find view / find partial view

    }
}
