using Bank.Core.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Bank.Core.Api.Infrastructure.Extensions;

internal static class MigrationExtensions
{
    public static IServiceProvider MigrateDatabase(this IServiceProvider services)
    {
        using var dbContext = services.CreateScope().ServiceProvider.GetRequiredService<CoreDbContext>();
        dbContext.Database.Migrate();

        return services;
    }
}