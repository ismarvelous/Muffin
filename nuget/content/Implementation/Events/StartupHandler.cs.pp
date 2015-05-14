using System.Reflection;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Muffin.Controllers;
using Muffin.Core;
using Muffin.Infrastructure;
using Umbraco.Core;
using Umbraco.Web;
using Umbraco.Web.WebApi;

namespace $rootnamespace$.Implementation.Events
{
	public class StartupHandler : FoundationEventHandler
	{
		public override void InitializeAtStartup(
			UmbracoApplicationBase umbracoApplication,
			ApplicationContext applicationContext,
			out IDependencyResolver resolver)
		{
			var builder = new ContainerBuilder();
			builder.RegisterApiControllers(typeof(UmbracoApiController).Assembly);
			builder.RegisterControllers(typeof(BaseController).Assembly);
			builder.RegisterControllers(Assembly.GetExecutingAssembly());

            builder.Register(s => new Mapper())
                .As<IMapper>()
                .InstancePerHttpRequest();

            
            var types = PluginManager.Current.ResolveTypes<ModelBase>();
            var factory = new CastleContentFactory(types);
            PublishedContentModelFactoryResolver.Current.SetFactory(factory); //remove this line if your like to depend fully on dynamic models

            builder.Register(s => factory)
                .As<IPublishedContentModelFactory>()
                .InstancePerHttpRequest();

            builder.Register(s => new SiteRepository(
                applicationContext.Services.ContentService,
                applicationContext.Services.MacroService,
                UmbracoContext.Current,
                factory))
                    .As<ISiteRepository>()
                    .InstancePerHttpRequest();


            var container = builder.Build();
			resolver = new AutofacDependencyResolver(container);
		}
	}
}