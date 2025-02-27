using Bank.Common.Models.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Bank.Common.Auth.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static Guid GetUserId(this ClaimsPrincipal user)
        {
            var value = user.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
            return user.Identity?.IsAuthenticated == null || value == null
                ? Guid.Empty
                : Guid.Parse(value);
        }

        public static Guid GetTokenId(this ClaimsPrincipal user)
        {
            var value = user.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
            return user.Identity?.IsAuthenticated == null || value == null
                ? Guid.Empty
                : Guid.Parse(value);
        }

        public static RoleType? GetUserMaxRole(this ClaimsPrincipal user)
        {
            var value = user.FindFirst(BankClaimTypes.MaxRole)?.Value;
            return user.Identity?.IsAuthenticated == null || value == null
                ? null
                : Enum.Parse<RoleType>(value);
        }
    }
}
