
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
using Our.Umbraco.Ditto;

namespace  Example.Implementation.ViewModels
{

	public partial class Base : ModelBase 
	{		
		public Base(IPublishedContent content): base (content) { }

		// properties..

		
		//no type converter specified
		public virtual string Titel { get; set; }
		
		//no type converter specified
		public virtual string Intro { get; set; }
		
		//no type converter specified
		public virtual string MainBody { get; set; }
		
		[TypeConverter(typeof(MediaPicker))]
		public virtual MediaModel Thumbnail { get; set; }
		
		[TypeConverter(typeof(MediaPicker))]
		public virtual MediaModel Afbeelding { get; set; }
		
		//no type converter specified
		public virtual string BrowserTitel { get; set; }
		
		//no type converter specified
		public virtual bool UmbracoNaviHide { get; set; }
		
		[DittoIgnore]
		public virtual IEnumerable<DynamicMacroModel> WidgetArea { get { return (new MacroContainer()).ConvertDataToSource(this.GetProperty("widgetArea")) as IEnumerable<DynamicMacroModel>; } }
		
		//no type converter specified
		public virtual string MetaDescription { get; set; }
		
		//no type converter specified
		public virtual string MetaKeywords { get; set; }
		
		//no type converter specified
		public virtual bool MetaRobots { get; set; }
		
		//no type converter specified
		public virtual string CanonicalTag { get; set; }
		
	}
	 
} 