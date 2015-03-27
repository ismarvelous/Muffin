using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Umbraco.Core;

namespace Muffin.CodeGenerator
{
    /// <summary>
    /// ViewModels Generator using Muffin base classes
    /// The models generator is using the uSync folder to find out which objects are in the Umbraco installation.
    /// </summary>
    public class ViewModelsGenerator 
    {
        private readonly string _uSyncPath;
        public ViewModelsGenerator(string uSyncPath)
        {
            _uSyncPath = uSyncPath;
        }

        public IEnumerable<DocumentType> GetDocumentTypes()
        {
            var results = new List<DocumentType>();
            var path = string.Format("{0}\\DocumentType", _uSyncPath);
            foreach (var classdir in Directory.GetDirectories(path))
            {
                results.AddRange(GetDocumentTypes(classdir));
            }

            return results;
        }

        private IEnumerable<DocumentType> GetDocumentTypes(string path)
        {
            var results = new List<DocumentType>();
            var serializer = new XmlSerializer(typeof(DocumentType));
            using (var reader = XmlReader.Create(string.Format("{0}\\def.config", path)))
            {
                results.Add((DocumentType)serializer.Deserialize(reader));
                foreach (var classdir in Directory.GetDirectories(path))
                {
                    results.AddRange(GetDocumentTypes(classdir));
                }
            }

            return results;
        }
    }

    /// <summary>
    /// Helper methods for creating class related strings which can be used by your T4 Template
    /// </summary>
    public static class ClassBuilderExtensions
    {
        #region Helpers

        public static string GetSafeClassName(this DocumentType doctype)
        {
            if (string.IsNullOrEmpty(doctype.Info.Alias))
                return string.Empty;

            return char.ToUpper(doctype.Info.Alias[0]) + doctype.Info.Alias.Substring(1);
        }

        public static string GetSafeBaseClassName(this DocumentType doctype)
        {
            // Check for empty string.
            if (string.IsNullOrEmpty(doctype.Info.Master))
                return "ModelBase";

            return char.ToUpper(doctype.Info.Master[0]) + doctype.Info.Master.Substring(1);
        }

        public static string GetConstructor(this DocumentType docType)
        {
            return string.Format("public {0}(IPublishedContent content): base (content) {{ }}", docType.GetSafeClassName());
        }

        #endregion
    }

    /// <summary>
    /// Helper methods for creating property related strings which can be used by your T4 Template
    /// </summary>
    public static class PropertyBuilderExtensions
    {
        public static string GetSafePropertyName(this GenericProperty property)
        {
            if (string.IsNullOrEmpty(property.Alias))
                return string.Empty;

            return char.ToUpper(property.Alias[0]) + property.Alias.Substring(1);
        }

        public static string GetPropertyType(this GenericProperty property)
        {
            //todo: make this list configurable...
            var typeList = new Dictionary<string, string>
            {
                {Constants.PropertyEditors.RelatedLinksAlias, "Func<IEnumerable<LinkModel>>"}, //late binder
                {Constants.PropertyEditors.ContentPickerAlias, "Func<IModel>"}, //late binder..
                {"Umbraco.Grid", "GridModel"},
                {Constants.PropertyEditors.MacroContainerAlias, "Func<IEnumerable<DynamicMacroModel>>"}, //late binder
                {Constants.PropertyEditors.MediaPickerAlias, "MediaModel"},
                {Constants.PropertyEditors.ImageCropperAlias, "CroppedImageModel"},
                {Constants.PropertyEditors.TrueFalseAlias, "bool"}
            };

            return typeList.ContainsKey(property.Type) ? typeList[property.Type] : "string"; 
        }

        public static string GetPropertyAtribute(this GenericProperty property)
        {
            //todo: make this list configurable...
            var typeList = new Dictionary<string, string>
            {
                {Constants.PropertyEditors.RelatedLinksAlias, "[TypeConverter(typeof(RelatedLinks))]"},
                {Constants.PropertyEditors.ContentPickerAlias, "[TypeConverter(typeof(ContentPicker))]"},
                {"Umbraco.Grid", "[TypeConverter(typeof(Grid))]"},
                {Constants.PropertyEditors.MacroContainerAlias, "[DittoIgnore]"},  //macro containers are not supported by Ditto..
                {Constants.PropertyEditors.MediaPickerAlias, "[TypeConverter(typeof(MediaPicker))]"},
                {Constants.PropertyEditors.ImageCropperAlias, "[TypeConverter(typeof(ImageCropper))]"},
            };

            return typeList.ContainsKey(property.Type) ? typeList[property.Type] : "//no type converter specified"; 
        }

        public static string GetPropertyAccessors(this GenericProperty property)
        {
            var typeList = new Dictionary<string, string> //PropertyEditors with special treatment
            {
                { Constants.PropertyEditors.MacroContainerAlias, string.Format("{{ get {{ return (new MacroContainer()).ConvertDataToSource(this.GetProperty(\"{0}\")) as Func<IEnumerable<DynamicMacroModel>>; }} }}", property.Alias) }
            };

            return typeList.ContainsKey(property.Type) ? typeList[property.Type] : "{ get; set; }";
        }
    }
}
