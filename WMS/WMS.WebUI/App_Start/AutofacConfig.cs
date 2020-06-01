using Autofac;
using Autofac.Integration.Mvc;
using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using WMS.WebUI.Common.IoC;

namespace WMS.WebUI
{
    public class AutofacConfig
    {
        public static void ConfigureContainer()
        {
            var builder = new ContainerBuilder();

            var configuration = GlobalConfiguration.Configuration;
            // Register dependencies in controllers

            var assembly = Assembly.Load("WMS.WebUI");
            builder.RegisterControllers(assembly);
            
            //Registring api controller

            // Register dependencies in filter attributes
            builder.RegisterFilterProvider();

            // Register dependencies in custom views
            builder.RegisterSource(new ViewRegistrationSource());

            // Register our Data dependencies
            builder.RegisterModule(new RepositoryModule());
            builder.RegisterModule(new ServiceModule());
            var container = builder.Build();

            // Set MVC DI resolver to use our Autofac container
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}