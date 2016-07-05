using System.Reflection;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Example.Implementation.Models;
using Muffin.Controllers;
using Muffin.Core;
using Muffin.Core.Models;
using Muffin.Infrastructure;
using Umbraco.Core;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;
using Umbraco.Web.WebApi;
using RegistrationExtensions = Autofac.Integration.Mvc.RegistrationExtensions;

namespace Example.Implementation.Events
{
	public class StartupHandler : FoundationEventHandler
	{
		public override void InitializeAtStartup(
			UmbracoApplicationBase umbracoApplication,
			ApplicationContext applicationContext,
			out IDependencyResolver resolver)
		{
            //1. Initialize your container..
			var builder = new ContainerBuilder();
			builder.RegisterApiControllers(typeof(UmbracoApiController).Assembly);
		    //builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
			builder.RegisterControllers(typeof(BaseController).Assembly);
			builder.RegisterControllers(Assembly.GetExecutingAssembly());
            //1.2 define the mapper
            builder.Register(s => new Mapper())
                .As<IMapper>().InstancePerHttpRequest();

            //2. use the castle content factory, or use your own.

            var factory = new CastleContentFactory(PluginManager.Current.ResolveTypes<PublishedContentModel>());
            PublishedContentModelFactoryResolver.Current.SetFactory(factory);

            //2.1 add the factory to the container.
            builder.Register(s => factory)
                .As<IPublishedContentModelFactory>()
                .InstancePerHttpRequest();

            //3. Register the Siterepository, you can use your own aswell.
            builder.Register(s => new SiteRepository(
                applicationContext.Services.ContentService,
                applicationContext.Services.MacroService,
                UmbracoContext.Current,
                factory,
                applicationContext.DatabaseContext.Database))
                    .As<ISiteRepository>()
                    .InstancePerHttpRequest();

            //4. Build the container...
            var container = builder.Build();
			resolver = new AutofacDependencyResolver(container);
		}
	}
}