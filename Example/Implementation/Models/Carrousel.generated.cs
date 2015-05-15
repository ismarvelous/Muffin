
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

	[PublishedContentModel("Carrousel")]
	public partial class Carrousel : ContainerBase 
	{		
		public Carrousel(IPublishedContent content): base (content) { }

		// properties..

		
		//no type converter specified
		public virtual string Rotatiesnelheid { get; set; }
		
	}
	 
} 