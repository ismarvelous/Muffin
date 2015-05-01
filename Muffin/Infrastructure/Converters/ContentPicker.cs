using System;
using System.ComponentModel;
using Muffin.Core;
using Muffin.Core.Models;
using Our.Umbraco.Ditto;
using Umbraco.Core;
using Umbraco.Core.Dynamics;
using Umbraco.Core.Models.PublishedContent;


namespace Muffin.Infrastructure.Converters
{
    public class ContentPicker : BaseTypeConverter, IConverter
    {
        public bool IsConverter(string editoralias)
        {
            return Constants.PropertyEditors.ContentPickerAlias.Equals(editoralias);
        }

        public object ConvertDataToSource(object source)
        {
            if (source is Func<IModel>)
                return source;

            if (source != null && !source.ToString().IsNullOrWhiteSpace())
            {
                Func<IModel> content = () => Repository.FindById<ModelBase>(Convert.ToInt32(source)).AsDynamic();
                return content;
            }

            Func<IModel> nullmodel = () => NullModel.Null;
            return nullmodel;
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, System.Type sourceType)
        {
            if (sourceType == typeof(int) || sourceType == typeof(string) || sourceType == typeof(Func<IModel>))
                return true;

            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {

            if (value is string || value is int || value is Func<IModel>)
            {
                return ConvertDataToSource(value);
            }

            return base.ConvertFrom(context, culture, value);
        }
    }
}
