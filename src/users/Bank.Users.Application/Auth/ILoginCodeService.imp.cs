using Bank.Common.Application.Z1all.ExecutionResult.StatusCode;
using Bank.Common.Auth.Configurations;
using Bank.Users.Application.Auth.Configurations;
using Bank.Users.Application.Auth.Helpers;
using Bank.Users.Application.Auth.Models;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Bank.Users.Application.Auth
{
    public class LoginCodeService : ILoginCodeService
    {
        private readonly IDatabase _redis;
        private readonly LoginCodeOptions _options;

        public LoginCodeService(
            IConnectionMultiplexer connectionMultiplexer,
            IOptions<LoginCodeOptions> options)
        {
            _redis = connectionMultiplexer.GetDatabase();
            _options = options.Value;
        }

        public async Task<ExecutionResult<LoginCodeDto>> CreateUserLoginCodeAsync(Guid userId)
        {
            var loginCode = TokensHelper.GenerateLoginCode();

            bool result = await _redis.StringSetAsync(loginCode, userId.ToString(), _options.LoginCodeTimeLife);
            if (!result)
            {
                return ExecutionResult<LoginCodeDto>.FromNotFound("SaveLoginCode", "Error when saving login code.");
            }

            return new LoginCodeDto()
            {
                Code = loginCode
            };
        }

        public async Task<ExecutionResult<Guid>> GetUserIdAsync(LoginCodeDto loginCode)
        {
            RedisValue result = await _redis.StringGetDeleteAsync(loginCode.Code);

            if (result.IsNullOrEmpty)
            {
                return ExecutionResult<Guid>.FromNotFound("GetLoginCode", "Login code does not found");
            }

            if (!Guid.TryParse(result, out Guid userId))
            {
                return ExecutionResult<Guid>.FromInternalServer("GetLoginCode", "Fail on parse user id");
            }

            return userId;
        }
    }
}
