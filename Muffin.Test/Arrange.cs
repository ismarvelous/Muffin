using Muffin.Core;
using Muffin.Core.Models;
using Umbraco.Core;
using Moq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web.Models;


namespace Muffin.Test
{
	/// <summary>
	/// Arrange helper class, contains some default mocks and datasources
	/// </summary>
	public class Arrange
	{
		public static Mock<IPublishedProperty> Property(string alias, object value)
		{
			var mockedProp = new Moq.Mock<IPublishedProperty>(MockBehavior.Strict);
			mockedProp.SetupGet(m => m.PropertyTypeAlias).Returns(alias);
			mockedProp.SetupGet(m => m.Value).Returns(value);
			//mockedProp.SetupGet(m => m.DataValue).Returns(value);
			mockedProp.SetupGet(m => m.HasValue).Returns(true);

			return mockedProp;
		}

		public static Mock<IModel> Content()
		{
			return Content("Lorem ipsum dolor");
		}

		public static Mock<IModel> Content(string name, bool umbracoNaviHide = false)
		{
			return Content(name, new List<IModel>(), null, umbracoNaviHide);
		}

		public static Mock<IModel> Content(string name, List<IModel> children, bool umbracoNaviHide = false)
		{
			return Content(name, children, new List<IPublishedProperty>(), umbracoNaviHide);
		}

		public static Mock<IModel> Content(string name, List<IPublishedProperty> properties, bool umbracoNaviHide = false)
		{
			return Content(name, new List<IModel>(), properties, umbracoNaviHide);
		}

		public static Mock<IModel> Content(string name,
			IEnumerable<IModel> children,
			List<IPublishedProperty> properties,
			bool umbracoNaviHide = false)
		{
			var mockedItem = new Moq.Mock<IModel>();
			mockedItem.SetupGet(m => m.Id).Returns(1);
			mockedItem.SetupGet(m => m.Name).Returns(name);
			mockedItem.SetupGet(m => m.Children).Returns(children);
		    mockedItem.SetupGet(m => m.Repository).Returns( DependencyResolver.Current.GetService<ISiteRepository>());

			//define properties
			var props = new List<IPublishedProperty>() 
                {
                    Property("title", name).Object,
                    Property("mainBody", "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Etiam gravida vehicula eleifend. Aenean dapibus ligula nisl, eget faucibus ligula vehicula non. Nullam pellentesque rhoncus rhoncus. Donec at ipsum mi. Phasellus eget augue eu lectus placerat lacinia. Sed justo libero, facilisis vitae lectus ut, venenatis interdum dui. In at tincidunt arcu, sit amet egestas elit. Vestibulum ac scelerisque augue. Aenean ut sagittis lacus, in aliquam nisi. Etiam ac massa nec purus malesuada sodales et sed neque. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae").Object,
                    Property(Constants.Conventions.Content.NaviHide, umbracoNaviHide).Object
                };

			if (properties != null) //and add custom properties.
			{
			    props.AddRange(properties);
			}

			//set properties in property collection
			mockedItem.SetupGet(m => m.Properties).Returns(props);

			//mock funcitons and setyp propertytypes
			foreach (var prop in props)
			{
				mockedItem.Setup(m => m.GetProperty(prop.PropertyTypeAlias))
					.Returns(prop);

				mockedItem.Setup(m => m.GetProperty(prop.PropertyTypeAlias, It.IsAny<bool>()))
					.Returns(prop);
			}

			return mockedItem;
		}

		public static List<ModelBase> BasicPages(ISiteRepository repository)
		{
			var ret = new List<ModelBase>()
			{
				{ new ModelBase(Content("Lorem page 1").Object) },
				{ new ModelBase(Content("Ipsum page 2").Object) },
				{ new ModelBase(Content("Dolor page 3").Object) },
				{ new ModelBase(Content("Sit page 4").Object) },
				{ new ModelBase(Content("Consectetur page 5").Object) },
				{ new ModelBase(Content("Donec page 6").Object) },
				{ new ModelBase(Content("Commodo page 7").Object) }
			};

			return ret;
		}
	}
}
