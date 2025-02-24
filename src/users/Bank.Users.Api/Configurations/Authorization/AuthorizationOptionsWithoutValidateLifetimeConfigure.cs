using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Bank.Users.Api.Configurations.Authorization
{
    public class AuthorizationOptionsWithoutValidateLifetimeConfigure : IConfigureOptions<AuthorizationOptions>
    {
        public void Configure(AuthorizationOptions options)
        {
            options.AddPolicy(JwtBearerWithoutValidateLifetimeDefaults.CheckOnlySignature, p => p.RequireAuthenticatedUser());
        }
    }
}
