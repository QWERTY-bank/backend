using StackExchange.Redis;

namespace Bank.Users.Application.Auth
{
    public class RefreshTokenStore : IRefreshTokenStore
    {
        private readonly IDatabase _redis;

        public RefreshTokenStore(IConnectionMultiplexer connectionMultiplexer)
        {
            _redis = connectionMultiplexer.GetDatabase();
        }

        public async Task<bool> SaveAsync(string refreshToken, Guid accessTokenJTI, TimeSpan refreshTimeLife)
        {
            bool keyExists = await _redis.KeyExistsAsync(accessTokenJTI.ToString());
            if (keyExists) return false;

            bool result = await _redis.StringSetAsync(accessTokenJTI.ToString(), refreshToken, refreshTimeLife);

            return result;
        }

        public async Task<bool> RemoveAsync(Guid accessTokenJTI)
        {
            bool result = await _redis.KeyDeleteAsync(accessTokenJTI.ToString());

            return result;
        }

        public async Task<bool> Exist(string refreshToken, Guid accessTokenJTI)
        {
            RedisValue result = await _redis.StringGetAsync(accessTokenJTI.ToString());

            if (result.IsNullOrEmpty) return false;
            return result == refreshToken;
        }
    }
}
