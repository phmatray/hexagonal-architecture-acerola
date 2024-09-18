using Autofac;

namespace Acerola.Infrastructure.InMemoryDataAccess;

public class Module : Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<Context>()
            .As<Context>()
            .SingleInstance();

        // Register all Types in InMemoryDataAccess namespace
        builder.RegisterAssemblyTypes(typeof(InfrastructureException).Assembly)
            .Where(type => type.Namespace.Contains("InMemoryDataAccess"))
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();
    }
}