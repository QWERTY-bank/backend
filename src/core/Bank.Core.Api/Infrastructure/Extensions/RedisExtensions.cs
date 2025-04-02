using RedLockNet;
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;
using StackExchange.Redis;

namespace Bank.Core.Api.Infrastructure.Extensions;

public static class RedisExtensions
{
    public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
    {
        var connection = configuration.GetConnectionString("Redis")!;
        
        var multiplexer = new List<RedLockMultiplexer> {
            ConnectionMultiplexer.Connect(connection)
        };
        
        var redLockFactory = RedLockFactory.Create(multiplexer);

        return services
            .AddSingleton<IDistributedLockFactory>(redLockFactory)
            .AddStackExchangeRedisCache(options => options.Configuration = connection);
    }
}