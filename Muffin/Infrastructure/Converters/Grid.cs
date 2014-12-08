using System;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.Dynamics;
using Muffin.Core.Models;

namespace Muffin.Infrastructure.Converters
{
    public class Grid : BaseConverter, IConverter
    {
        public override bool IsConverter(PublishedPropertyType propertyType)
        {
            return IsConverter(propertyType.PropertyEditorAlias);
        }

        public bool IsConverter(string editoralias)
        {
            return "Umbraco.Grid".Equals(editoralias);
        }

        public override object ConvertDataToSource(PublishedPropertyType propertyType, object source, bool preview)
        {
            return ConvertDataToSource(source);
        }

        public object ConvertDataToSource(object source)
        {
            try
            {
                var ret = DynamicGridModel.Create(source.ToString());
                return ret;
            }
            catch (Exception ex)
            {
                //todo: log exception...
                return DynamicNull.Null;
            }
        }
    }
}
