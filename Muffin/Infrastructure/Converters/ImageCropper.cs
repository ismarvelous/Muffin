using System;
using Muffin.Core.Models;
using Umbraco.Core.Models.PublishedContent;

namespace Muffin.Infrastructure.Converters
{
    /// <summary>
    /// More info on the image cropper: http://our.umbraco.org/documentation/Using-Umbraco/Backoffice-Overview/Property-Editors/Built-in-Property-Editors-v7/Image-Cropper
    /// </summary>
    public class ImageCropper : BaseConverter, IConverter
    {
        public override bool IsConverter(PublishedPropertyType propertyType)
        {
            return IsConverter(propertyType.PropertyEditorAlias);
        }

        public bool IsConverter(string editoralias)
        {
            return "Umbraco.ImageCropper".Equals(editoralias) 
                || "media".Equals(editoralias); //support for grid media aswell.
        }

        public override object ConvertDataToSource(PublishedPropertyType propertyType, object source, bool preview)
        {
            return ConvertDataToSource(source);
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
    }
}
