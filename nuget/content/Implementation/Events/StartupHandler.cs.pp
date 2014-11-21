using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Muffin.Core;
using Muffin.Events;
using Muffin.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Web;
using Umbraco.Web.WebApi;

namespace $rootnamespace$.Implementation.Events
{
	public class StartupHandler : FoundationEventHandler
	{
		public override void InitializeAtStartup(
			Umbraco.Core.UmbracoApplicationBase umbracoApplication,
			Umbraco.Core.ApplicationContext applicationContext,
			out System.Web.Mvc.IDependencyResolver resolver)
		{
			var builder = new ContainerBuilder();
			builder.RegisterApiControllers(typeof(UmbracoApiController).Assembly);
			builder.RegisterControllers(typeof(Muffin.Controllers.DynamicBaseController).Assembly);
			builder.RegisterControllers(System.Reflection.Assembly.GetExecutingAssembly());

			builder.Register(s => new SiteRepository(
				applicationContext.Services.ContentService,
				new UmbracoHelper(UmbracoContext.Current)))
					.As<ISiteRepository>()
					.InstancePerHttpRequest();

			var container = builder.Build();
			resolver = new Autofac.Integration.Mvc.AutofacDependencyResolver(container);
		}
	}
}