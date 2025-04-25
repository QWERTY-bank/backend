using Bank.Common.Api.Configurations;
using Bank.Common.Api.Cors;
using Bank.Users.Api;
using Bank.Users.Application;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCorsConfigure();

builder.ConfigureAppsettings();
builder.Services.AddApiUsersConfigurations();
builder.Services.AddUsersServices();
builder.Services.AddApiAutoMapperProfiles();
builder.Services.AddApplicationAutoMapperProfiles();
builder.AddUsersDbContext();
builder.AddRedisDb();

var resourceBuilder = ResourceBuilder.CreateDefault()
    .AddService("users");

builder.Services.AddOpenTelemetry()
    .WithMetrics(metrics =>
    {
        metrics.SetResourceBuilder(resourceBuilder)
               .AddHttpClientInstrumentation()
               .AddAspNetCoreInstrumentation()
               .AddPrometheusExporter();
    })
    .WithTracing(tracing =>
    {
        tracing.SetResourceBuilder(resourceBuilder)
               .AddHttpClientInstrumentation()
               .AddAspNetCoreInstrumentation()
               .AddConsoleExporter()
               .AddOtlpExporter(options =>
               {
                   options.Endpoint = new Uri("http://tempo:4317");
                   options.Protocol = OtlpExportProtocol.Grpc;
               });
    });
    //.WithLogging(logging =>
    //{
    //    logging.SetResourceBuilder(resourceBuilder)
    //           .AddOtlpExporter(options =>
    //           {
    //               options.Endpoint = new Uri("http://localhost:4310");
    //           });
    //});

var app = builder.Build();

app.Services.AddAutoMigration();
app.Services.AddDatabaseSeed();

app.UseSwagger();
app.UseSwaggerUI();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();
app.MapPrometheusScrapingEndpoint();

app.Run();