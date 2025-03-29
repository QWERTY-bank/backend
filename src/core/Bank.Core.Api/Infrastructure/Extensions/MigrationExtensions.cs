using Bank.Core.Domain.Accounts;
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

        await AddCurrenciesAsync(dbContext);
        await AddCreditAccountAsync(dbContext);

        await dbContext.SaveChangesAsync();
    }
    
    private static async Task AddCurrenciesAsync(CoreDbContext dbContext)
    {
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
    }

    private static async Task AddCreditAccountAsync(CoreDbContext dbContext)
    {
        var unitId = Guid.Parse("a99fb2a5-c52e-4168-8ac9-b28878d3b407");

        var accountExists = await dbContext.UnitAccounts
            .AnyAsync(account => account.UnitId == unitId);

        if (accountExists)
        {
            return;
        }
        
        var unitAccounts = Enum.GetValues<CurrencyCode>()
            .Select(code => new UnitAccountEntity
        {
            Title = $"Аккаунт кредитов {code}",
            UnitId = unitId,
            AccountCurrencies =
            [
                new AccountCurrencyEntity
                {
                    Code = code,
                    Value = 100000000000
                }
            ]
        });

        dbContext.UnitAccounts.AddRange(unitAccounts);
    }
}