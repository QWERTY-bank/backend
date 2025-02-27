using Bank.Common.Api.Configurations.Others;
using Bank.Common.Api.Configurations.Swagger;
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
    }
}
