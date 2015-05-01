using System.ComponentModel;
using Muffin.Core.Models;
using Umbraco.Core;
using Umbraco.Core.Models.PublishedContent;

namespace Muffin.Infrastructure.Converters
{
    public class MediaPicker : BaseTypeConverter, IConverter
	{
        public bool IsConverter(string editoralias)
        {
            return Constants.PropertyEditors.MediaPickerAlias.Equals(editoralias);
        }

	    public object ConvertDataToSource(object source)
	    {
	        if (source is ICropImageModel)
	            return source;

;            int val;
            if (int.TryParse(source.ToString(), out val))
            {
                var media = Repository.FindMediaById(val);

                if (media != null)
                    return media;
            }

            return DynamicNullMedia.Null;
	    }

        public override bool CanConvertFrom(ITypeDescriptorContext context, System.Type sourceType)
        {
            return sourceType == typeof(string) || sourceType == typeof(int) || sourceType == typeof(ICropImageModel) || base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is string || value is int || value is ICropImageModel)
            {
                return ConvertDataToSource(value);
            }

            return base.ConvertFrom(context, culture, value);
        }
	}
}
