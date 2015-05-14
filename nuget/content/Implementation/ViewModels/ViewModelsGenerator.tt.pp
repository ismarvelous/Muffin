<#@ template debug="false" hostspecific="true" language="C#" #>
<#@ assembly name="System.Core" #>
<#@include file="MultipleOutputHelper.ttinclude"#>

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
	var namesp ="Example.Implementation.ViewModels";
	var gen = new ViewModelsGenerator(Host.ResolvePath("../../uSync"));
	var doctypes = gen.GetDocumentTypes();
    var manager = Manager.Create(Host, GenerationEnvironment);
#>
	
<#+ manager.StartHeader(); #>

// -- AUTO GENERATED CONTENT, DO NOT MODIFY --		
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

namespace  <#= namesp #>
{
<#+ manager.EndBlock(); #>
    
	<#+foreach (var itm in doctypes) { #>
	<#+ manager.StartNewFile(itm.Info.Alias + ".generated.cs"); #>

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