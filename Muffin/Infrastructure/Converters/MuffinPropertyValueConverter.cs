using System;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using Muffin.Core;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.PropertyEditors;

namespace Muffin.Infrastructure.Converters
{
    public class MuffinPropertyValueConverter : PropertyValueConverterBase, IPropertyValueConverterMeta
    {
        protected ISiteRepository Repository => DependencyResolver.Current.GetService<ISiteRepository>();

        public bool IsConverter(PublishedPropertyType propertyType)
        {
            if (!(ConfigurationManager.AppSettings["Muffin.ValueConverterMode"] != null &&
                ConfigurationManager.AppSettings["Muffin.ValueConverterMode"].ToLower()
                    .Equals("propertyvalueconvertermode"))) return false;

            var assembly = typeof(IConverter).Assembly;
            var types = assembly.GetTypes().Where(type => type != typeof(IConverter) && typeof(IConverter).IsAssignableFrom(type) && type != typeof(BaseTypeConverter)).ToList();
            return types.Select(type => (IConverter)Activator.CreateInstance(type)).Any(instance => instance.IsConverter(propertyType.PropertyEditorAlias));
        }

        public override object ConvertDataToSource(PublishedPropertyType propertyType, object source, bool preview)
        {
            return Repository.ConvertPropertyValue(propertyType.PropertyEditorAlias, source);
        }

        public override object ConvertSourceToObject(PublishedPropertyType propertyType, object source, bool preview)
        {
            return Repository.ConvertPropertyValue(propertyType.PropertyEditorAlias, source);
        }
        
        public virtual Type GetPropertyValueType(PublishedPropertyType propertyType)
        {
            var assembly = typeof(IConverter).Assembly;
            var types = assembly.GetTypes().Where(type => type != typeof(IConverter) && typeof(IConverter).IsAssignableFrom(type) && type != typeof(BaseTypeConverter)).ToList();

            foreach (var type in types)
            {
                var instance = (IConverter)Activator.CreateInstance(type);
                if (instance.IsConverter(propertyType.PropertyEditorAlias))
                {
                    return instance.ReturnType;
                }
            }

            return typeof(object);
        }

        public virtual PropertyCacheLevel GetPropertyCacheLevel(PublishedPropertyType propertyType, PropertyCacheValue cacheValue)
        {
            return PropertyCacheLevel.Content;
        }
    }
}
