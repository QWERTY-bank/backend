using System.Reflection;
using Bank.Core.Domain.Accounts;
using Bank.Core.Domain.Currencies;
using Bank.Core.Domain.Transactions;
using Microsoft.EntityFrameworkCore;

namespace Bank.Core.Persistence;

public class CoreDbContext : DbContext
{
    private const string BankCoreSchema = "bank_core";

    public CoreDbContext(DbContextOptions<CoreDbContext> options) : base(options)
    {
    }
    
    public DbSet<AccountBaseEntity> Accounts { get; set; }
    public DbSet<TransactionEntity> Transactions { get; set; }
    public DbSet<CurrencyEntity> Currencies { get; set; }
    public DbSet<AccountCurrencyEntity> AccountCurrencies { get; set; }

    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var assembly = Assembly.GetExecutingAssembly();
        modelBuilder.ApplyConfigurationsFromAssembly(assembly);
        modelBuilder.HasDefaultSchema(BankCoreSchema);
    }
}