using Acerola.WebApi.Filters;
using Autofac;
using Autofac.Configuration;
using Autofac.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

// Configure Autofac as the DI container
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.RegisterModule(new ConfigurationModule(builder.Configuration));
});

// Add configuration files and environment variables
builder.Configuration.AddJsonFile("autofac.json", true, true);
builder.Configuration.AddEnvironmentVariables();

// Add logging using Serilog
builder.Host.UseSerilog((context, loggerConfiguration) =>
{
    loggerConfiguration.MinimumLevel.Debug()
        .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
        .Enrich.FromLogContext()
        .WriteTo.File(Path.Combine(context.HostingEnvironment.ContentRootPath, "logs/log-.log"),
            rollingInterval: RollingInterval.Day);
});

// Configure services
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policyBuilder
        => policyBuilder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(DomainExceptionFilter));
    options.Filters.Add(typeof(ValidateModelAttribute));
});

// Swagger configuration
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = builder.Configuration["App:Title"],
        Version = builder.Configuration["App:Version"],
        Description = builder.Configuration["App:Description"],
        TermsOfService = new Uri(builder.Configuration["App:TermsOfService"])
    });

    // Replaces DescribeAllEnumsAsStrings() since it's deprecated
    options.CustomSchemaIds(type => type.FullName);

    // Optional: Including XML comments if needed
    var xmlFile = $"{typeof(Program).Assembly.GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
});

var app = builder.Build();

// Middleware configuration
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// Enable CORS
app.UseCors("CorsPolicy");

// Enable routing and controllers
app.UseRouting();
app.MapControllers();

// Enable Swagger
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
});

app.Run();
