using Bank.Core.Application;
using Bank.Core.Persistence;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Bank.Core.Api.Infrastructure.Extensions;

internal static class DatabaseExtensions
{
    public static IServiceCollection AddCoreDatabase(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Core");

        var dataSource = new NpgsqlDataSourceBuilder(connectionString)
            .EnableDynamicJson()
            .Build();
        
        services.AddDbContextPool<CoreDbContext>(opt =>
        {
            opt
                .UseNpgsql(dataSource)
                .UseSnakeCaseNamingConvention();
        });
        
        services.AddScoped<ICoreDbContext>(provider => provider.GetRequiredService<CoreDbContext>());

        return services;
    }
}