
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

	public partial class Home : Base 
	{		
		public Home(IPublishedContent content): base (content) { }

		// properties..

		
		[TypeConverter(typeof(RelatedLinks))]
		public virtual Func<IEnumerable<LinkModel>> SocialNetworks { get; set; }
		
		[TypeConverter(typeof(ContentPicker))]
		public virtual Func<IModel> CarrouselContainer { get; set; }
		
	}
	 
} 