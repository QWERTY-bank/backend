using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Bank.Common.OpenTelemetry
{
    public static class OpenTelemetryExtensions
    {
        public static IServiceCollection AddOpenTelemetry(this IServiceCollection services, IConfiguration configuration, string serviceName)
        {
            OpenTelemetryOptions openTelemetryOptions = new();
            configuration.GetRequiredSection("OpenTelemetry").Bind(openTelemetryOptions);

            var resourceBuilder = ResourceBuilder.CreateDefault()
                .AddService(serviceName);

            services.AddOpenTelemetry()
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
                               options.Endpoint = new Uri(openTelemetryOptions.TracingGrpcEndpoint);
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

            return services;
        }

        public static void UseOpenTelemetry(this WebApplication app)
        {
            app.MapPrometheusScrapingEndpoint();
        }
    }
}
