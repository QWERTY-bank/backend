using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Bank.Users.Api.Configurations.Authorization
{
    /// <summary>
    /// Конфигуратор схемы авторизации без проверки времени жизни jwt токена
    /// </summary>
    public class AuthorizationOptionsWithoutValidateLifetimeConfigure : IConfigureOptions<AuthorizationOptions>
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void Configure(AuthorizationOptions options)
        {
            options.AddPolicy(JwtBearerWithoutValidateLifetimeDefaults.CheckOnlySignature, p => p.RequireAuthenticatedUser());
        }
    }
}
