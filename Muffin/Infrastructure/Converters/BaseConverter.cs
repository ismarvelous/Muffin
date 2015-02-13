using System;
using System.ComponentModel;
using System.Web.Mvc;
using Muffin.Core;
using Umbraco.Core.PropertyEditors;
using Umbraco.Web;

namespace Muffin.Infrastructure.Converters
{
    [Obsolete("Make use of the new BaseTypeConverter")]
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

    public abstract class BaseTypeConverter : TypeConverter
    {
        protected ISiteRepository Repository;
        protected IMapper Mapper;
        protected UmbracoHelper Helper;

        protected BaseTypeConverter()
        {
            Initialize(DependencyResolver.Current.GetService<ISiteRepository>(), DependencyResolver.Current.GetService<IMapper>());
        }

        protected BaseTypeConverter(ISiteRepository rep, IMapper mapper)
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
