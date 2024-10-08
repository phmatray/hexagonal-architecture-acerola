﻿using Autofac;

namespace Acerola.Infrastructure.DapperDataAccess;

public class Module : Autofac.Module
{
    public string ConnectionString { get; set; }

    protected override void Load(ContainerBuilder builder)
    {
        // Register all Types in MongoDataAccess namespace
        builder.RegisterAssemblyTypes(typeof(InfrastructureException).Assembly)
            .Where(type => type.Namespace?.Contains("DapperDataAccess") == true)
            .WithParameter("connectionString", ConnectionString)
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();
    }
}