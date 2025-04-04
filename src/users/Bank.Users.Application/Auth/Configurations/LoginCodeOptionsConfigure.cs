using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Bank.Users.Application.Auth.Configurations
{
    public class LoginCodeOptionsConfigure(IConfiguration configuration) : IConfigureOptions<LoginCodeOptions>
    {
        private readonly string valueKey = "LoginCode";
        private readonly IConfiguration _configuration = configuration;

        public void Configure(LoginCodeOptions options) => _configuration.GetSection(valueKey).Bind(options);
    }
}
