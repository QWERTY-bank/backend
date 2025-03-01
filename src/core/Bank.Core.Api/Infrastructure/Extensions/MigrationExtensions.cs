using Bank.Core.Domain.Currencies;
using Bank.Core.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Bank.Core.Api.Infrastructure.Extensions;

internal static class MigrationExtensions
{
    public static async Task MigrateDatabaseAsync(this IServiceProvider services)
    {
        await using var dbContext = services.CreateScope().ServiceProvider
            .GetRequiredService<CoreDbContext>();
        
        await dbContext.Database.MigrateAsync();
    }
    
    public static async Task SeedDataAsync(this IServiceProvider services)
    {
        await using var dbContext = services.CreateScope().ServiceProvider
            .GetRequiredService<CoreDbContext>();
        
        var currencies = await dbContext.Currencies
            .ToDictionaryAsync(currency => currency.Code);
        
        foreach (var currencyCode in Enum.GetValues<CurrencyCode>())
        {
            if (currencies.ContainsKey(currencyCode))
            {
                continue;
            }
            
            var currency = new CurrencyEntity
            {
                Code = currencyCode
            };

            dbContext.Add(currency);
        }

        await dbContext.SaveChangesAsync();
    }
}