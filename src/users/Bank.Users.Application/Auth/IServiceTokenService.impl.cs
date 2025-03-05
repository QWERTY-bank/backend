using Bank.Users.Application.Auth.Configurations;
using Bank.Users.Application.Auth.Helpers;
using Bank.Users.Application.Auth.Models;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Z1all.ExecutionResult.StatusCode;

namespace Bank.Users.Application.Auth
{
    public class ServiceTokenService : IServiceTokenService
    {
        private readonly JwtServiceOptions _jwtOptions;

        public ServiceTokenService(IOptions<JwtServiceOptions> options)
        {
            _jwtOptions = options.Value;
        }

        public ExecutionResult<ServiceTokenDto> CreateServiceTokens()
        {
            Guid accessTokenJTI = Guid.NewGuid();

            List<Claim> claims = GetClaims(accessTokenJTI);

            var tokenExpired = DateTime.UtcNow.AddMinutes(_jwtOptions.AccessTokenTimeLifeMinutes);
            var token = TokensHelper.GenerateJwtToken(claims, tokenExpired, _jwtOptions.SecretKey);

            return ExecutionResult<ServiceTokenDto>.FromSuccess(new ServiceTokenDto
            {
                Token = token,
            });
        }

        private List<Claim> GetClaims(Guid accessTokenJTI)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, accessTokenJTI.ToString()),
                new Claim(ClaimTypes.NameIdentifier, "a99fb2a5-c52e-4168-8ac9-b28878d3b407"),
            };

            return claims;
        }
    }
}
