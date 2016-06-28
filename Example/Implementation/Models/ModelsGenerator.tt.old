<#@ template debug="false" hostspecific="true" language="C#" #>
<#@ assembly name="System.Core" #>
<#@include file="MultipleOutputHelper.ttinclude"#>


<#@ assembly name="$(ProjectDir)bin\Umbraco.Core.dll" #>
<#@ import namespace="Umbraco.Core"#>
<#@ assembly name="$(ProjectDir)bin\Muffin.dll" #>
<#@ import namespace="Muffin.CodeGenerator"#>

<#@ output extension=".cs" #>

<#
	//todo: variable namespace and usync folder
	//todo: skip file ViewModelsGen.cs this is not needed..
    GenerateModels();
#>

<#+
private void GenerateModels()
{
	//1. Configuration
	var namesp ="Example.Implementation.Models";
	var source = Host.ResolvePath("../../uSync");

	//1.2 Define property types ("$alias", "$type")
	var types = new Dictionary<string, string>
    {
        {Constants.PropertyEditors.RelatedLinksAlias, "IEnumerable<LinkModel>"},
        {Constants.PropertyEditors.ContentPickerAlias, "IModel"},
        {Constants.PropertyEditors.MultiNodeTreePickerAlias, "IEnumerable<IModel>"}, 
        {"Umbraco.Grid", "GridModel"},
        {Constants.PropertyEditors.MacroContainerAlias, "IEnumerable<DynamicMacroModel>"},
        {Constants.PropertyEditors.MediaPickerAlias, "ICropImageModel"},
        {Constants.PropertyEditors.ImageCropperAlias, "ICropImageModel"},
        {Constants.PropertyEditors.TrueFalseAlias, "bool"}

    };

	//1.3 Define Typeconverters ("$alias", "$conveter")
	var converters = new Dictionary<string, string>
    {
        {Constants.PropertyEditors.RelatedLinksAlias, "[TypeConverter(typeof(RelatedLinks))]"},
        {Constants.PropertyEditors.ContentPickerAlias, "[TypeConverter(typeof(ContentPicker))]"},
        {"Umbraco.Grid", "[TypeConverter(typeof(Grid))]"},
        {Constants.PropertyEditors.MacroContainerAlias, "[TypeConverter(typeof(MacroContainer))]"},
        {Constants.PropertyEditors.MediaPickerAlias, "[TypeConverter(typeof(MediaPicker))]"},
        {Constants.PropertyEditors.ImageCropperAlias, "[TypeConverter(typeof(ImageCropper))]"},
        {Constants.PropertyEditors.MultiNodeTreePickerAlias, "[TypeConverter(typeof(MultiNodeTreePicker))]"},
    };

	var gen = new ModelsGenerator(source, types, converters);
	var doctypes = gen.GetDocumentTypes();
    var manager = Manager.Create(Host, GenerationEnvironment);
#>
	
<#+ manager.StartHeader(); #>

// -- AUTO GENERATED CONTENT ON, DO NOT MODIFY --		
// <copyright file="ViewModelsGenerator.tt" company="Marvelous IT Solutions">
// Copyright © Marvelous IT Solutions. All Rights Reserved.
// this file is auto-generated, don't change anything in the created files.
// </copyright>  

using System;
using System.Collections.Generic;
using System.ComponentModel;
using Muffin.Core.Models;
using Muffin.Infrastructure.Converters;
using Umbraco.Core.Models;
using Muffin.Core;
using Umbraco.Core.Models.PublishedContent;

namespace  <#= namesp #>
{
<#+ manager.EndBlock(); #>
    
	<#+foreach (var itm in doctypes) { #>
	<#+ manager.StartNewFile(itm.Info.Alias + ".generated.cs"); #>

	[PublishedContentModel("<#=itm.Info.Alias#>")]
	public partial class <#=itm.GetSafeClassName()#> : <#=itm.GetSafeBaseClassName()#> 
	{		
		<#=itm.GetConstructor()#>

		// properties..

		<#+foreach(var prop in itm.GenericProperties) { #>

		<#=prop.GetPropertyAtribute() #>
		public virtual <#=prop.GetPropertyType()#> <#=prop.GetSafePropertyName()#> <#=prop.GetPropertyAccessors()#>
		<#+}#>

	}
	<#+ manager.EndBlock();#>
<#+}#><#+ manager.StartFooter();#> 
} <#+ manager.EndBlock(); #>

<#+ manager.Process(true);#>

<#+}#>