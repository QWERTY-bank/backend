using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace UserService.Infrastructure.Identity.Configurations.Authorization
{
    public class AuthorizationOptionsWithoutValidateLifetimeConfigure : IConfigureOptions<AuthorizationOptions>
    {
        public void Configure(AuthorizationOptions options)
        {
            options.AddPolicy(JwtBearerWithoutValidateLifetimeDefaults.CheckOnlySignature, p => p.RequireAuthenticatedUser());
        }
    }
}
