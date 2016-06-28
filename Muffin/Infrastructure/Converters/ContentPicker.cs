using System;
using System.ComponentModel;
using System.Globalization;
using Muffin.Core.Models;
using Umbraco.Core;

namespace Muffin.Infrastructure.Converters
{
    public class ContentPicker : BaseTypeConverter
    {
        public override Type ReturnType => typeof(IModel);

        public override bool IsConverter(string editoralias)
        {
            return Constants.PropertyEditors.ContentPickerAlias.Equals(editoralias);
        }

        public override object ConvertDataToSource(object source)
        {
            if (source is IModel)
                return source;

            if (source != null && !source.ToString().IsNullOrWhiteSpace())
            {
                return Repository.FindById(Convert.ToInt32(source));
            }

            return null;
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(int) || sourceType == typeof(string) || sourceType == typeof(IModel))
                return true;

            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value == null)
                return null;

            if (value is string || value is int || value is IModel)
            {
                return ConvertDataToSource(value);
            }

            return base.ConvertFrom(context, culture, value);
        }
    }
}
