using Muffin.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Muffin.Core;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.PropertyEditors;
using Umbraco.Core.Services;
using Umbraco.Web;

namespace Muffin.Infrastructure.Converters
{
	public abstract class BaseConverter : PropertyValueConverterBase
    {
		protected ISiteRepository Repository;
		protected UmbracoHelper Helper;

	    protected BaseConverter() 
        {
			Initialize(DependencyResolver.Current.GetService<ISiteRepository>());
        }

	    protected BaseConverter(ISiteRepository rep)
        {
            Initialize(rep);
        }

        protected void Initialize(ISiteRepository rep)
        {
			Repository = rep;
        }
	}
}
