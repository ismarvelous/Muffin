using System.Collections.Generic;
using Muffin.Core.Models;
using Umbraco.Core;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;

namespace Muffin.Infrastructure.Converters
{
    /// <summary>
    /// Multi node tree picker converter
    /// </summary>
    public class MultiNodeTreePicker : BaseConverter
    {
        /// <summary>
        /// Check converter type
        /// </summary>
        /// <param name="propertyType"></param>
        /// <returns></returns>
        public override bool IsConverter(PublishedPropertyType propertyType)
        {
            return Constants.PropertyEditors.MultiNodeTreePickerAlias.Equals(propertyType.PropertyEditorAlias);
        }

        /// <summary>
        /// Convert to data source
        /// </summary>
        /// <param name="propertyType"></param>
        /// <param name="source"></param>
        /// <param name="preview"></param>
        /// <returns></returns>
        public override object ConvertDataToSource(PublishedPropertyType propertyType, object source, bool preview)
        {
            var ret = new List<IModel>(); //return value

            if(source != null && !string.IsNullOrEmpty(source.ToString()))
            {
                foreach (var item in source.ToString().Split(',')) // source contains a comma seperate value
                {
                    int id = 0;
                    if(int.TryParse(item, out id))
                    {
                        ret.Add(Mapper.AsDynamicIModel(Repository.FindById<ModelBase>(id)));
                    }
                }
            }
            return ret;
        }
    }
}
