﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Muffin.Core.Models;
using Umbraco.Core;
using Umbraco.Core.Dynamics;
using Umbraco.Core.Models.PublishedContent;

namespace Muffin.Infrastructure.Converters
{
    public class RelatedLinks : BaseConverter, IConverter
    {
        public override bool IsConverter(PublishedPropertyType propertyType)
        {
            return IsConverter(propertyType.PropertyEditorAlias);
        }

        public bool IsConverter(string editoralias)
        {
            return Constants.PropertyEditors.RelatedLinksAlias.Equals(editoralias);
        }

        public override object ConvertDataToSource(PublishedPropertyType propertyType, object source, bool preview)
        {
            return ConvertDataToSource(source);
        }

        public object ConvertDataToSource(object source)
        {
            var ret = new List<LinkModel>(); //return value
            var arr = Newtonsoft.Json.JsonConvert.DeserializeObject(source.ToString()) as Newtonsoft.Json.Linq.JArray;

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
    }
}
