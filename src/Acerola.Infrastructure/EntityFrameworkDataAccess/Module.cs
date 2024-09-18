﻿using Autofac;
using Microsoft.EntityFrameworkCore;

namespace Acerola.Infrastructure.EntityFrameworkDataAccess;

public class Module : Autofac.Module
{
    public string? ConnectionString { get; set; }

    protected override void Load(ContainerBuilder builder)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DbContext>();
        optionsBuilder.UseSqlServer(ConnectionString);
        optionsBuilder.EnableSensitiveDataLogging();

        builder.RegisterType<Context>()
            .WithParameter(new TypedParameter(typeof(DbContextOptions), optionsBuilder.Options))
            .InstancePerLifetimeScope();

        // Register all Types in MongoDataAccess namespace
        builder.RegisterAssemblyTypes(typeof(InfrastructureException).Assembly)
            .Where(type => type.Namespace?.Contains("EntityFrameworkDataAccess") == true)
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();
    }
}