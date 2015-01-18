using Autofac;
using Autofac.Integration.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Web.Mvc;
using Muffin.Controllers;
using Muffin.Core;

namespace Muffin.Test
{
    public class BaseTestClass
    {
        public Mock<ISiteRepository> Repository { get; set; }
        public Mock<IMapper> Mapper { get; set; }

        [TestInitialize]
        public void Initialize()
        {

            var builder = new ContainerBuilder();

            builder.RegisterControllers(typeof(BaseController).Assembly);
            builder.RegisterControllers(System.Reflection.Assembly.GetExecutingAssembly());

            Repository = new Mock<ISiteRepository>();
            builder.Register(s => Repository.Object)
                .As<ISiteRepository>();

            Mapper = new Mock<IMapper>();
            builder.Register(s => Repository.Object)
                .As<ISiteRepository>();

            var container = builder.Build();
            var lifetimeScopeProvider = new StubLifetimeScopeProvider(container);
            var resolver = new AutofacDependencyResolver(container, lifetimeScopeProvider);
            DependencyResolver.SetResolver(resolver);
            // Now you can use DependencyResolver.Current in
            // tests without getting the web request lifetime exception.
        }
    }
}
