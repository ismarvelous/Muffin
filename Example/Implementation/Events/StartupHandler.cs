using System.Reflection;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Muffin.Controllers;
using Muffin.Core;
using Muffin.Core.Models;
using Muffin.Infrastructure;
using Umbraco.Core;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;
using Umbraco.Web.WebApi;

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
			builder.RegisterControllers(typeof(BaseController).Assembly);
			builder.RegisterControllers(Assembly.GetExecutingAssembly());
            //1.2 define the mapper
            builder.Register(s => new Mapper())
                .As<IMapper>()
                .InstancePerHttpRequest();

            //2. use the castle content factory, or use your own.
            var factory = new CastleContentFactory(PluginManager.Current.ResolveTypes<ModelBase>());
            PublishedContentModelFactoryResolver.Current.SetFactory(factory); //remove this line if your like to depend fully on dynamic models

            //2.1 add the factory to the container.
            builder.Register(s => factory)
                .As<IPublishedContentModelFactory>()
                .InstancePerHttpRequest();

            //3. Register the Siterepository, you can use your own aswell.
            builder.Register(s => new SiteRepository(
                applicationContext.Services.ContentService,
                applicationContext.Services.MacroService,
                UmbracoContext.Current,
                factory))
                    .As<ISiteRepository>()
                    .InstancePerHttpRequest();

            //4. Build the container...
            var container = builder.Build();
			resolver = new AutofacDependencyResolver(container);
		}
	}
}