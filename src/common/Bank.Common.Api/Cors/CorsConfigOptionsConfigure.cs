using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Bank.Common.Api.Cors
{
    public class CustomCorsOptionsConfigure(IConfiguration configuration) : IConfigureOptions<CorsConfigOptions>
    {
        private readonly string valueKey = "Cors";
        private readonly IConfiguration _configuration = configuration;

        public void Configure(CorsConfigOptions options) => _configuration.GetSection(valueKey).Bind(options);
    }
}
