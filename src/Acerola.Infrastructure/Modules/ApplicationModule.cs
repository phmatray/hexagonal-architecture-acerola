using Autofac;

namespace Acerola.Infrastructure.Modules;

public class ApplicationModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        // Register all Types in Acerola.Application
        builder.RegisterAssemblyTypes(typeof(Application.ApplicationException).Assembly)
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();
    }
}