using Bank.Common.Auth;
using Bank.Common.Auth.Configurations;
using Bank.Users.Application.Auth.Helpers;
using Bank.Users.Application.Auth.Models;
using Bank.Users.Domain.Users;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using Z1all.ExecutionResult.StatusCode;

namespace Bank.Users.Application.Auth
{
    public class TokensService : ITokensService
    {
        private readonly IRefreshTokenStore _refreshTokenStore;
        private readonly ILogger<TokensService> _logger;
        private readonly JwtOptions _jwtOptions;

        public TokensService(
            IRefreshTokenStore refreshTokenStore,
            ILogger<TokensService> logger,
            IOptions<JwtOptions> options)
        {
            _refreshTokenStore = refreshTokenStore;
            _logger = logger;
            _jwtOptions = options.Value;
        }

        public async Task<ExecutionResult<TokensDTO>> CreateUserTokensAsync(UserEntity user)
        {
            Guid accessTokenJTI = Guid.NewGuid();

            List<Claim> claims = GetClaims(user, accessTokenJTI);

            var accessTokenExpired = DateTime.UtcNow.AddMinutes(_jwtOptions.AccessTokenTimeLifeMinutes);
            var accessToken = TokensHelper.GenerateJwtToken(claims, accessTokenExpired, _jwtOptions.SecretKey);

            var refreshTokenTimeLife = new TimeSpan(_jwtOptions.RefreshTokenTimeLifeDays, 0, 0, 0);
            var refreshToken = TokensHelper.GenerateRefreshToken();

            var saveRefreshTokenResult = await _refreshTokenStore.SaveAsync(refreshToken, accessTokenJTI, refreshTokenTimeLife);
            if (!saveRefreshTokenResult)
            {
                _logger.LogCritical($"Error when saving a refresh token for a user with id = '{user.Id}'.");
                return ExecutionResult<TokensDTO>.FromInternalServer("CreateTokens", "Error when creating tokens.");
            }

            return new TokensDTO()
            {
                Access = accessToken,
                AccessExpiredDateTime = accessTokenExpired,

                Refresh = refreshToken,
                RefreshExpiredDateTime = DateTime.UtcNow.Add(refreshTokenTimeLife)
            };
        }

        public async Task<ExecutionResult> RemoveRefreshAsync(Guid accessTokenJTI)
        {
            bool result = await _refreshTokenStore.RemoveAsync(accessTokenJTI);
            if (!result)
            {
                _logger.LogInformation($"Token with accessTokenJTI = '{accessTokenJTI}' does not found.");
                return ExecutionResult.FromBadRequest("RemoveRefresh", "The tokens have already been deleted.");
            }

            return ExecutionResult.FromSuccess();
        }

        public async Task<ExecutionResult> RemoveRefreshAsync(string refreshToken, Guid accessTokenJTI)
        {
            bool refreshTokenExist = await _refreshTokenStore.Exist(refreshToken, accessTokenJTI);
            if (!refreshTokenExist)
            {
                _logger.LogInformation($"Token with accessTokenJTI = '{accessTokenJTI}' does not found.");
                return ExecutionResult.FromBadRequest("RemoveRefresh", "Refresh token does not found");
            }

            bool result = await _refreshTokenStore.RemoveAsync(accessTokenJTI);
            if (!result)
            {
                _logger.LogInformation($"Token with accessTokenJTI = '{accessTokenJTI}' does not found.");
                return ExecutionResult.FromBadRequest("RemoveRefresh", "The tokens have already been deleted.");
            }

            return ExecutionResult.FromSuccess();
        }

        private List<Claim> GetClaims(UserEntity user, Guid accessTokenJTI)
        {
            var roles = user.Roles.Select(x => x.Type);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, accessTokenJTI.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(BankClaimTypes.MaxRole, roles.Max().ToString()),
                new Claim(BankClaimTypes.Roles, JsonSerializer.Serialize(roles.Select(x => x.ToString()))),
            };

            return claims;
        }
    }
}
