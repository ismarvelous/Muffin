using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;
using Umbraco.Core;

namespace Muffin.CodeGenerator
{
    /// <summary>
    /// ViewModels Generator using Muffin base classes
    /// The models generator is using the uSync folder to find out which objects are in the Umbraco installation.
    /// </summary>
    public class ModelsGenerator 
    {
        private readonly string _uSyncPath;
        //todo: clean up code so these statics are not needed.
        internal static IDictionary<string, string> TypeList;
        internal static IDictionary<string, string> Converters;

        public ModelsGenerator(string uSyncPath, IDictionary<string, string> typeList, IDictionary<string, string> converters)
        {

            _uSyncPath = uSyncPath;
            TypeList = typeList;
            Converters = converters;
        }

        public IEnumerable<DocumentType> GetDocumentTypes()
        {
            var results = new List<DocumentType>();
            var path = string.Format("{0}\\data\\DocumentType", _uSyncPath);
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
            return string.IsNullOrEmpty(doctype.Info.Alias) ? string.Empty : GetSafeClassName(doctype.Info.Alias);
        }

        private static string GetSafeClassName(string alias)
        {
            return Regex.Replace(alias, @"(^\w)|(_\w)", (m) => m.Value.Substring(m.Value.Length > 1 ? 1 : 0).ToUpper(),
                RegexOptions.IgnoreCase);
        }

        public static string GetSafeBaseClassName(this DocumentType doctype)
        {
            // Check for empty string.
            return string.IsNullOrEmpty(doctype.Info.Master) ? "ModelBase" : GetSafeClassName(doctype.Info.Master);
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
            var typeList = ModelsGenerator.TypeList;
            return typeList.ContainsKey(property.Type) ? 
                typeList[property.Type] : 
                "string"; //default type. 
        }

        public static string GetPropertyAtribute(this GenericProperty property)
        {
            var attr = ModelsGenerator.Converters;
            return attr.ContainsKey(property.Type) ? 
                attr[property.Type] : 
                "";  //no type converters defined.
        }

        public static string GetPropertyAccessors(this GenericProperty property)
        {
            return "{ get; set; }";
        }
    }
}
