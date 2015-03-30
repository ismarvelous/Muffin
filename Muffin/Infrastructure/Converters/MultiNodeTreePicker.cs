﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using Muffin.Core.Models;
using Umbraco.Core;

namespace Muffin.Infrastructure.Converters
{
    /// <summary>
    /// Multi node tree picker converter
    /// </summary>
    public class MultiNodeTreePicker : BaseTypeConverter, IConverter
    {
        public bool IsConverter(string alias)
        {
            return Constants.PropertyEditors.MultiNodeTreePickerAlias.Equals(alias);
        }

        public object ConvertDataToSource(object source)
        {
            if(source != null && !string.IsNullOrEmpty(source.ToString()))
            {
                Func<IEnumerable<IModel>> func = () => ConvertToIEnumerable(source.ToString().Split(',')); //return value;
                return func;
            }
            else
            {
                Func<IEnumerable<IModel>> func = () => new List<IModel>(); //return value;
                return func;
            }
        }

        protected IEnumerable<IModel> ConvertToIEnumerable(string[] arr)
        {
            var ret = new List<IModel>(); //return value
            foreach (var item in arr) // source contains a comma seperate value
            {
                var id = 0;
                if (int.TryParse(item, out id))
                {
                    ret.Add(Mapper.AsDynamicIModel(Repository.FindById<ModelBase>(id)));
                }
            }

            return ret;
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                return ConvertDataToSource(value);
            }

            return base.ConvertFrom(context, culture, value);
        }
    }
}
