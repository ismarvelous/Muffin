using Autofac;
using Autofac.Integration.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Web.Mvc;
using Muffin.Controllers;
using Muffin.Core;
using Muffin.Core.Models;
using Muffin.Infrastructure;
using Umbraco.Core;
using Umbraco.Core.Models.PublishedContent;

namespace Muffin.Test
{
    public class BaseTestClass
    {
        public Mock<ISiteRepository> Repository { get; set; }
        public Mock<IMapper> Mapper { get; set; }
        public Mock<IPublishedContentModelFactory> ModelFactory { get; set; }

        [TestInitialize]
        public void Initialize()
        {

            //var types = PluginManager.Current.ResolveTypes<ModelBase>();
            //var factory = new MuffinPublishedContentModelFactory(types);
            //PublishedContentModelFactoryResolver.Current = new PublishedContentModelFactoryResolver();
            //PublishedContentModelFactoryResolver.Current.SetFactory(factory);

            var builder = new ContainerBuilder();

            builder.RegisterControllers(typeof(BaseController).Assembly);
            builder.RegisterControllers(System.Reflection.Assembly.GetExecutingAssembly());

            Repository = new Mock<ISiteRepository>();
            builder.Register(s => Repository.Object)
                .As<ISiteRepository>();

            Mapper = new Mock<IMapper>();
            builder.Register(s => Repository.Object)
                .As<ISiteRepository>();

            ModelFactory = new Mock<IPublishedContentModelFactory>();
            builder.Register(s => ModelFactory.Object)
                .As<IPublishedContentModelFactory>();

            var container = builder.Build();
            var lifetimeScopeProvider = new StubLifetimeScopeProvider(container);
            var resolver = new AutofacDependencyResolver(container, lifetimeScopeProvider);
            DependencyResolver.SetResolver(resolver);
            // Now you can use DependencyResolver.Current in
            // tests without getting the web request lifetime exception.
        }
    }
}
