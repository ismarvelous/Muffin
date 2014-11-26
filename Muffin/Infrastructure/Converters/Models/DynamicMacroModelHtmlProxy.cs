using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using Muffin.Core.Models;
using umbraco.cms.businesslogic.macro;
using Umbraco.Core.Services;
using Umbraco.Web.Macros;

namespace Muffin.Infrastructure.Converters.Models
{
    /// <summary>
    /// Proxy Model for DynamicMacroModel implementing IHtmlString to render a macro while calling IHtmlString
    /// </summary>
    public class DynamicMacroModelHtmlProxy : DynamicMacroModel, IHtmlString
    {
        public DynamicMacroModelHtmlProxy(DynamicMacroModel source,
            int pageId, string macroValue) : base (source.Macro, source.Source, source.Repository)
        {
            _macroValue = macroValue;
            _pageId = pageId;
        }

        private readonly string _macroValue;
        private readonly int _pageId;

        public override string ToString()
        {
            //todo: or is it better to implement our own IMacroEngine

            if (!string.IsNullOrWhiteSpace(_macroValue)) //_macrovalue is not used anymore to render the content, however this check is still valid.
            {
                var engine = new PartialViewMacroEngine();
                //do not use the overload Macro(macro, content), because umbraco.cms.businesslogic.macro is obsolete..
                var m = new MacroModel //map IMacro source model to MacroModel
                {
                    Id = Macro.Id,
                    Alias = Macro.Alias,
                    Name = Macro.Name,
                    Properties = Source.ToList(),
                    ScriptName = Macro.ScriptPath
                };

                return engine.Execute(m, Repository.FindById(_pageId));
            }

            //Old / obsolete implementation
            //if (!string.IsNullOrWhiteSpace(_macroValue))
            //    return umbraco.library.RenderMacroContent(_macroValue,
            //        _pageId);

            return Macro.Alias; //default;
        }

        public string ToHtmlString()
        {
            return new HtmlString(ToString()).ToString();
        }
    }
}
