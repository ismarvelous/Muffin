using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using Muffin.Core.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Umbraco.Core;

namespace Muffin.Infrastructure.Converters
{
    public class RelatedLinks : BaseTypeConverter, IConverter
    {
        public bool IsConverter(string editoralias)
        {
            return Constants.PropertyEditors.RelatedLinksAlias.Equals(editoralias);
        }

        public object ConvertDataToSource(object source)
        {
            var ret = new List<LinkModel>(); //return value
            var arr = JsonConvert.DeserializeObject(source.ToString()) as JArray;

            if (arr != null)
            {
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
                            NewWindow = (bool) item["newWindow"]
                        });
                    }
                }
            }

            return ret;
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || sourceType == typeof(JArray) || base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string || value is JArray)
            {
                return ConvertDataToSource(value);
            }

            return base.ConvertFrom(context, culture, value);
        }
    }
}
