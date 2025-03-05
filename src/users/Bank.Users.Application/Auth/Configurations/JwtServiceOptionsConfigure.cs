using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Bank.Users.Application.Auth.Configurations
{
    public class JwtServiceOptionsConfigure(IConfiguration configuration) : IConfigureOptions<JwtServiceOptions>
    {
        private readonly string valueKey = "JwtServiceAuthentication";
        private readonly IConfiguration _configuration = configuration;

        public void Configure(JwtServiceOptions options) => _configuration.GetSection(valueKey).Bind(options);
    }
}

