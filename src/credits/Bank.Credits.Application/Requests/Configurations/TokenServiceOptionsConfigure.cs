using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Bank.Credits.Application.Requests.Configurations
{
    public class TokenServiceOptionsConfigure(IConfiguration configuration) : IConfigureOptions<TokenServiceOptions>
    {
        private readonly string valueKey = "TokenService";
        private readonly IConfiguration _configuration = configuration;

        public void Configure(TokenServiceOptions options) => _configuration.GetSection(valueKey).Bind(options);
    }
}
