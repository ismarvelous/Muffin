using System;
using System.ComponentModel;
using Muffin.Core.Models;
using Newtonsoft.Json.Linq;
using Umbraco.Core.Models.PublishedContent;

namespace Muffin.Infrastructure.Converters
{
    /// <summary>
    /// More info on the image cropper: http://our.umbraco.org/documentation/Using-Umbraco/Backoffice-Overview/Property-Editors/Built-in-Property-Editors-v7/Image-Cropper
    /// </summary>
    public class ImageCropper : BaseTypeConverter, IConverter
    {
        public bool IsConverter(string editoralias)
        {
            return "Umbraco.ImageCropper".Equals(editoralias) 
                || "media".Equals(editoralias); //support for grid media aswell.
        }

        public object ConvertDataToSource(object source)
        {
            try
            {
                return CroppedImageModel.Create(source.ToString());
            }
            catch(Exception ex)
            {
                //todo: log exception...
                return DynamicNullMedia.Null;
            }
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, System.Type sourceType)
        {
            return sourceType == typeof(string) || sourceType == typeof(JObject) || base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is string || value is JObject)
            {
                return ConvertDataToSource(value);
            }

            return base.ConvertFrom(context, culture, value);
        }
    }
}
