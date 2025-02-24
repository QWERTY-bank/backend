using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Bank.Common.Api.Configurations.Authorization
{
    public class AuthorizationOptionsConfigure : IConfigureOptions<AuthorizationOptions>
    {
        public void Configure(AuthorizationOptions options)
        {
            options.DefaultPolicy = new AuthorizationPolicyBuilder()
                .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .Build();
        }
    }
}
