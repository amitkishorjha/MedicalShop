using Autofac;
using System.Linq;
using System.Reflection;

namespace WMS.WebUI.Common.IoC
{
    public class ServiceModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterTypes(Assembly.Load("WMS.Service")
                .GetExportedTypes()
                .Where(x => x.Name.EndsWith("Service")).ToArray())
                .AsImplementedInterfaces();

            base.Load(builder);
        }
    }
}