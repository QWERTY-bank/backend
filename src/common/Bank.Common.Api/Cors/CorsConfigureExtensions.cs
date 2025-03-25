using Microsoft.Extensions.DependencyInjection;

namespace Bank.Common.Api.Cors
{
    public static class CorsConfigureExtensions
    {
        public static IServiceCollection AddCorsConfigure(this IServiceCollection services)
        {
            services.ConfigureOptions<CustomCorsOptionsConfigure>();

            services.AddCors();

            return services;
        }
    }
}
