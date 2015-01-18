using System;
using Muffin.Core;
using Muffin.Core.Models;
using Umbraco.Core;
using Umbraco.Web.Macros;
using Umbraco.Core.Models;
using System.Web.Mvc;
using Muffin.Infrastructure;
using Our.Umbraco.Ditto;
using Umbraco.Web;

namespace Muffin.Mvc
{
    public class DynamicMacroViewPage : PartialViewMacroPage
	{
		public ISiteRepository Repository {get; private set;}
        public IMapper Mapper { get; private set; }

        public DynamicMacroViewPage()
            : base()
        {
            Repository = DependencyResolver.Current.GetService<ISiteRepository>();
            Mapper = DependencyResolver.Current.GetService<IMapper>();
        }

		private dynamic _currentPage;
		public new dynamic CurrentPage
		{
			get
			{
                // ReSharper disable once ConvertIfStatementToNullCoalescingExpression : For readability
				if (_currentPage == null)
				{
				    _currentPage =  Mapper.AsDynamicIModel((base.CurrentPage as IPublishedContent).As<ModelBase>());
				}

				return _currentPage;
			}
		}

		private dynamic _macro;
		/// <summary>
		/// Access to all Macro parameters by using this dynamic property.
		/// Returns a "DynamicMacroModel"
		/// </summary>
        public virtual dynamic Macro
        {
            get
            {
                // ReSharper disable once ConvertIfStatementToNullCoalescingExpression : For readability
				if (_macro == null)
				{
					_macro = Repository.FindMacroByAlias(Model.MacroAlias, (int)CurrentPage.Id, Model.MacroParameters);
				    // when using the themeengine, _marco.Macro.ScriptPath is not correct here, don't use from here..
				}

				return _macro;
            }
        }

        public override void Execute()
        {
            throw new NotImplementedException();
        }
	}
}
