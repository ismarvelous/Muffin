
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

namespace  Example.Implementation.Models
{

	[PublishedContentModel("Home")]
	public partial class Home : Base 
	{		
		public Home(IPublishedContent content): base (content) { }

		// properties..

		
		[TypeConverter(typeof(ContentPicker))]
		public virtual IModel CarrouselContainer { get; set; }
		
		[TypeConverter(typeof(MultiNodeTreePicker))]
		public virtual IEnumerable<IModel> Quickmenu { get; set; }
		
		[TypeConverter(typeof(RelatedLinks))]
		public virtual IEnumerable<LinkModel> SocialNetworks { get; set; }
		
	}
	 
} 