using Microsoft.EntityFrameworkCore;

namespace Bank.Core.Application;

internal static class CoreDbContextExtensions
{
    public static Task<int> SelectAccountsForUpdate(
        this ICoreDbContext dbContext,
        IReadOnlyCollection<long> accountIds,
        CancellationToken cancellationToken)
    {
        var accountIdsString = string.Join(",", accountIds);
        
        var command = $"SELECT id FROM bank_core.account_entity WHERE id IN ({accountIdsString}) FOR UPDATE;";
        return dbContext.Database.ExecuteSqlRawAsync(command, cancellationToken);
    }
    public static Task<int> SelectAccountForUpdate(
        this ICoreDbContext dbContext,
        long accountId,
        CancellationToken cancellationToken)
    {
        var command = $"SELECT id FROM bank_core.account_entity WHERE id = {accountId} FOR UPDATE;";
        return dbContext.Database.ExecuteSqlRawAsync(command, cancellationToken);
    }
}