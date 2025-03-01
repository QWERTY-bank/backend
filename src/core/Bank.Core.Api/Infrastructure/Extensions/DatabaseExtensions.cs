using Bank.Core.Application;
using Bank.Core.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Bank.Core.Api.Infrastructure.Extensions;

internal static class DatabaseExtensions
{
    public static IServiceCollection AddCoreDatabase(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Core");
        
        services.AddDbContextPool<CoreDbContext>(opt =>
        {
            opt
                .UseNpgsql(connectionString)
                .UseSnakeCaseNamingConvention();
        });
        
        services.AddScoped<ICoreDbContext>(provider => provider.GetRequiredService<CoreDbContext>());

        return services;
    }
}