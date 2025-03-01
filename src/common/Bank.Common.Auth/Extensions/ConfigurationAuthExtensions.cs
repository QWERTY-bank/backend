using Bank.Common.Auth.Configurations;
using Bank.Common.Auth.RequirementHandlers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Bank.Common.Auth.Extensions
{
    public static class ConfigurationAuthExtensions
    {
        public static AuthenticationBuilder AddJwtAuthentication(this IServiceCollection services)
        {
            services.ConfigureOptions<AuthorizationOptionsConfigure>();

            services.ConfigureOptions<JwtBearerOptionsConfigure>();
            services.ConfigureOptions<JwtOptionsConfigure>();

            services.AddHttpContextAccessor();
            services.AddScoped<IAuthorizationHandler, MinimalRoleTypeHandler>();
            services.AddSingleton<IAuthorizationPolicyProvider, PolicyProvider>();

            return services.AddAuthentication()
                    .AddJwtBearer();
        }
    }
}
