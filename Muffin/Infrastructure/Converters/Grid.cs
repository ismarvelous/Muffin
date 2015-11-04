using System;
using System.ComponentModel;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.Dynamics;
using Muffin.Core.Models;
using Newtonsoft.Json.Linq;

namespace Muffin.Infrastructure.Converters
{
    public class Grid : BaseTypeConverter, IConverter
    {
        public bool IsConverter(string editoralias)
        {
            return "Umbraco.Grid".Equals(editoralias);
        }

        public object ConvertDataToSource(object source)
        {
            if (source is GridModel)
                return source;

            try
            {
                var json = source.ToString();

                if(!string.IsNullOrWhiteSpace(json))
                {
                    var ret = GridModel.Create(json);
                    return ret;
                }
            }
            catch (Exception ex)
            {
                //todo: log exception...
            }

            return DynamicNull.Null;
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, System.Type sourceType)
        {
            return sourceType == typeof(JObject) || base.CanConvertFrom(context, sourceType) || sourceType == typeof(GridModel);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is JObject || value is GridModel)
            {
                return ConvertDataToSource(value);
            }

            return null;
        }
    }
}
