using Bank.Core.Domain.Accounts;
using Bank.Core.Domain.Currencies;
using Bank.Core.Domain.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Bank.Core.Application;

public interface ICoreDbContext
{
    public DbSet<AccountBaseEntity> Accounts { get; }
    public DbSet<TransactionEntity> Transactions { get; }
    public DbSet<CurrencyEntity> Currencies { get; }
    public DbSet<AccountCurrencyEntity> AccountCurrencies { get; }
    public DbSet<PersonalAccountEntity> PersonalAccounts { get; }
    public DbSet<UnitAccountEntity> UnitAccounts { get;  }
    
    public DatabaseFacade Database { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}