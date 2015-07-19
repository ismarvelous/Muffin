
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
using Umbraco.Core.Models.PublishedContent;

namespace  Example.Implementation.Models
{

	[PublishedContentModel("Base")]
	public partial class Base : ModelBase 
	{		
		public Base(IPublishedContent content): base (content) { }

		// properties..

		
		
		public virtual string Titel { get; set; }
		
		
		public virtual string Intro { get; set; }
		
		
		public virtual string MainBody { get; set; }
		
		[TypeConverter(typeof(MediaPicker))]
		public virtual ICropImageModel Thumbnail { get; set; }
		
		[TypeConverter(typeof(MediaPicker))]
		public virtual ICropImageModel Afbeelding { get; set; }
		
		
		public virtual string BrowserTitel { get; set; }
		
		
		public virtual bool UmbracoNaviHide { get; set; }
		
		[TypeConverter(typeof(MacroContainer))]
		public virtual IEnumerable<DynamicMacroModel> WidgetArea { get; set; }
		
		
		public virtual string MetaDescription { get; set; }
		
		
		public virtual string MetaKeywords { get; set; }
		
		
		public virtual bool MetaRobots { get; set; }
		
		
		public virtual string CanonicalTag { get; set; }
		
	}
	 
} 