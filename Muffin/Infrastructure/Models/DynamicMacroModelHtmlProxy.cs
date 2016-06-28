using System.Linq;
using System.Web;
using Muffin.Core.Models;
using umbraco.cms.businesslogic.macro;
using Umbraco.Web.Macros;

namespace Muffin.Infrastructure.Models
{
    /// <summary>
    /// Proxy Model for DynamicMacroModel implementing IHtmlString to render a macro while calling IHtmlString
    /// </summary>
    internal class DynamicMacroModelHtmlProxy : DynamicMacroModel, IHtmlString
    {
        internal DynamicMacroModelHtmlProxy(DynamicMacroModel source,
            int pageId, string macroValue) : base (source.Macro, source.Source, source.Repository)
        {
            _macroValue = macroValue;
            _pageId = pageId;
        }

        private readonly string _macroValue;
        private readonly int _pageId;

        public override string ToString()
        {
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

                return engine.Execute(m, Repository.FindById<IModel>(_pageId));
            }

            return Macro.Alias; //default;
        }

        public string ToHtmlString()
        {
            return new HtmlString(ToString()).ToString();
        }
    }
}
