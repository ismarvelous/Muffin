﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using Muffin.Core.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Umbraco.Core;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.PropertyEditors;

namespace Muffin.Infrastructure.Converters
{
    /// <summary>
    /// Convert a MacroContainer
    /// </summary>
    public class RelatedLinks : BaseTypeConverter, IConverter
    {
        public bool IsConverter(string editoralias)
        {
            return Constants.PropertyEditors.RelatedLinksAlias.Equals(editoralias);
        }

        public object ConvertDataToSource(object source)
        {
            try
            {
                if (source is IEnumerable<LinkModel>)
                    return source;

                var arr = JsonConvert.DeserializeObject(source.ToString()) as JArray;
                return arr != null ? ConvertToIEnumerable(arr) : new List<LinkModel>();
            }
            catch (StackOverflowException ex)
            {
                Debug.WriteLine(ex.Message);
                return new List<LinkModel>(); //return value;
            }
        }

        protected IEnumerable<LinkModel> ConvertToIEnumerable(JArray arr)
        {
            var ret = new List<LinkModel>();
            foreach (var item in arr)
            {
                int id;
                if (int.TryParse(item["link"].ToString(), out id)) //internal
                {
                    ret.Add(new LinkModel
                    {
                        Title = item["title"].ToString(),
                        Url = Repository.FriendlyUrl(id),
                        NewWindow = (item["newWindow"].ToString() == "1")
                    });
                }
                else //external link
                {
                    ret.Add(new LinkModel
                    {
                        Title = item["title"].ToString(),
                        Url = item["link"].ToString(),
                        NewWindow = (bool)item["newWindow"]
                    });
                }
            }

            return ret;
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || sourceType == typeof(JArray) || sourceType == typeof(IEnumerable<LinkModel>) || base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string || value is JArray || value is IEnumerable<LinkModel>)
            {
                return ConvertDataToSource(value);
            }

            return base.ConvertFrom(context, culture, value);
        }
    }

    /// <summary>
    /// DIRTY FIX: override Umbraco's RelatedLinksEditorValue converter. The original Core implementation is calling 
    /// .NiceUrl.. which results in a StackoverflowException when used together with the ditto factory.
    /// </summary>
    [PropertyValueType(typeof(JArray))]
    [PropertyValueCache(PropertyCacheValue.All, PropertyCacheLevel.Content)]
    public class RelatedLinksEditorValueConvertor : PropertyValueConverterBase
    {
        public override bool IsConverter(PublishedPropertyType propertyType)
        {
            return Constants.PropertyEditors.RelatedLinksAlias.Equals(propertyType.PropertyEditorAlias);
        }

        public override object ConvertDataToSource(PublishedPropertyType propertyType, object source, bool preview)
        {
            return source;
        }

        public override object ConvertSourceToXPath(PublishedPropertyType propertyType, object source, bool preview)
        {
            return source;
        }
    }
}
