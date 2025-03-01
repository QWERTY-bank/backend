using Bank.Common.Api.Configurations.Others;
using Bank.Common.Api.Configurations.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bank.Common.Api.Configurations
{
    public static class ConfigurationApiExtensions
    {
        public static IServiceCollection AddModalStateConfigure(this IServiceCollection services)
        {
            services.ConfigureOptions<ModalStateOptionsConfigure>();

            return services;
        }

        public static IServiceCollection AddSwaggerConfigure(this IServiceCollection services)
        {
            services.ConfigureOptions<SwaggerGenOptionsConfigure>();

            return services;
        }

        public static void ConfigureAppsettings(this WebApplicationBuilder builder)
        {
            var environment = Environment.GetEnvironmentVariable("RUNTIME_ENVIRONMENT");

            builder.Configuration
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true);

            if (environment == "Docker")
            {
                builder.Configuration.AddJsonFile("appsettings.Docker.json", optional: true, reloadOnChange: true);
            }
        }
    }
}
