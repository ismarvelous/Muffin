﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using Muffin.Core;
using Muffin.Core.Models;
using Muffin.Infrastructure.Models;
using Umbraco.Core;
using Umbraco.Core.Dynamics;
using Umbraco.Core.IO;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;

namespace Muffin.Infrastructure.Converters
{
    /// <summary>
    /// Convert a MacroContainer
    /// </summary>
    public class MacroContainer : BaseTypeConverter
    {
        public override Type ReturnType => typeof(IEnumerable<DynamicMacroModel>);

        public override bool IsConverter(string editoralias)
        {
            return Constants.PropertyEditors.MacroContainerAlias.Equals(editoralias);
        }

        //todo: this doesn't support the umbraco macro build in cache strategie, find a way to cache the outputs..

        public override object ConvertDataToSource(object source)
        {
            if (source is IEnumerable<DynamicMacroModel>)
                return source;

            var content = source is string
                ? source as string
                : source is IPublishedProperty ? ((IPublishedProperty) source).DataValue.ToString() : null;

            if (!string.IsNullOrWhiteSpace(content) && UmbracoContext.Current != null && UmbracoContext.Current.PageId.HasValue)
            {
                return ConvertToIEnumerable(content);
            }

            return new List<DynamicMacroModelHtmlProxy>();
        }

        protected IEnumerable<DynamicMacroModel> ConvertToIEnumerable(string content)
        {
            var macros = Regex.Matches(content, "(\\<\\?UMBRACO_MACRO.+?(\\/>))");
            var ret = new List<DynamicMacroModelHtmlProxy>();

            // ReSharper disable once LoopCanBeConvertedToQuery : for readability
            foreach (Match match in macros)
            {
                var parameters = Regex.Matches(match.Value, "(\\w+)=(\"[^<>\"]*\"|\'[^<>\']*\'|\\w+)").Cast<Match>().ToList();
                var alias = parameters.Where(val => val.Groups[1].Value.Equals("macroAlias"))
                    .Select(val => val.Groups[2].Value.Replace("\"", string.Empty).Replace("'", string.Empty)).FirstOrDefault();

                var values = parameters.Where(val => !val.Groups[1].Value.Equals("macroAlias"))
                    .ToDictionary(val => val.Groups[1].Value, val => val.Groups[2].Value.Replace("\"", string.Empty).Replace("'", string.Empty) as object);

                #region select corret scriptpath
                //todo: check file availability, otherwise use default path as a fallback.
                var macro = Repository.FindMacroByAlias(alias, UmbracoContext.Current.PageId.Value, values);
                var macroPartialsPath = string.Concat(SystemDirectories.MvcViews, "/MacroPartials/");
                if (macro.Macro.ScriptPath.Contains(macroPartialsPath))
                {
                    macro.Macro.ScriptPath = macro.Macro.ScriptPath.Replace(macroPartialsPath,
                        string.Format("{0}/{1}/", "~/Views", "MacroPartials"));
                }
                #endregion

                var macroProxy = new DynamicMacroModelHtmlProxy(macro,
                    UmbracoContext.Current.PageId.Value, match.Value);

                ret.Add(macroProxy);
            }

            return ret;
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || sourceType == typeof(IEnumerable<DynamicMacroModel>) || base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is string || value is IEnumerable<DynamicMacroModel> || value is IPublishedProperty)
            {
                return ConvertDataToSource(value);
            }

            return base.ConvertFrom(context, culture, value);
        }
    }
}
