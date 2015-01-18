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
	    protected IMapper Mapper;
		protected UmbracoHelper Helper;

	    protected BaseConverter() 
        {
            Initialize(DependencyResolver.Current.GetService<ISiteRepository>(), DependencyResolver.Current.GetService<IMapper>());
        }

	    protected BaseConverter(ISiteRepository rep, IMapper mapper)
        {
            Initialize(rep, mapper);
        }

        protected void Initialize(ISiteRepository rep, IMapper map)
        {
			Repository = rep;
            Mapper = map;
        }
	}
}
