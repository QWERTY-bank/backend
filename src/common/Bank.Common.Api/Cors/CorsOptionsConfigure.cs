using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.Options;

namespace Bank.Common.Api.Cors
{
    public class CorsOptionsConfigure(IOptions<CorsConfigOptions> options) : IConfigureNamedOptions<CorsOptions>
    {
        private readonly CorsConfigOptions _options = options.Value;

        public void Configure(string? name, CorsOptions options)
        {
            Configure(options);
        }

        public void Configure(CorsOptions options)
        {
            options.AddDefaultPolicy(policy =>
            {
                policy
                    .WithOrigins(_options.AllowedOrigins.Split())
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        }
    }
}
