using System;
using System.ComponentModel;
using System.Web.Mvc;
using Muffin.Core;
using Umbraco.Core.PropertyEditors;
using Umbraco.Web;

namespace Muffin.Infrastructure.Converters
{
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
