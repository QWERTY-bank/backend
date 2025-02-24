using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Bank.Common.Api.Configurations.Others;
using Bank.Common.Api.Configurations.Authorization;

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

        public static AuthenticationBuilder AddJwtAuthentication(this IServiceCollection services)
        {
            services.ConfigureOptions<AuthorizationOptionsConfigure>();

            services.ConfigureOptions<JwtBearerOptionsConfigure>();
            services.ConfigureOptions<JwtOptionsConfigure>();

            return services.AddAuthentication()
                    .AddJwtBearer();
        }
    }
}
