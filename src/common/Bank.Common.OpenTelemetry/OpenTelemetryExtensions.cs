using LokiLoggingProvider.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Confluent.Kafka.Extensions.OpenTelemetry;

namespace Bank.Common.OpenTelemetry
{
    public static class OpenTelemetryExtensions
    {
        public static void AddOpenTelemetry(this WebApplicationBuilder builder, string serviceName)
        {
            OpenTelemetryOptions openTelemetryOptions = new();
            builder.Configuration.GetRequiredSection("OpenTelemetry").Bind(openTelemetryOptions);

            var resourceBuilder = ResourceBuilder.CreateDefault()
                .AddService(serviceName);

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
                           .AddConfluentKafkaInstrumentation()
                           .AddOtlpExporter(options =>
                           {
                               options.Endpoint = new Uri(openTelemetryOptions.TracingGrpcEndpoint);
                               options.Protocol = OtlpExportProtocol.Grpc;
                           });
                });

            builder.Logging.AddLoki(loggerOptions =>
            {
                loggerOptions.Client = PushClient.Grpc;
                loggerOptions.StaticLabels.JobName = serviceName;
                loggerOptions.Formatter = Formatter.Json;
                loggerOptions.Grpc.Address = openTelemetryOptions.LoggingGrpcEndpoint;
            });
        }

        public static void UseOpenTelemetry(this WebApplication app)
        {
            app.MapPrometheusScrapingEndpoint();
        }
    }
}
