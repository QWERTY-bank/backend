using Bank.Core.Domain.Accounts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bank.Core.Persistence.Accounts;

public class AccountBaseEntityConfiguration : IEntityTypeConfiguration<AccountBaseEntity>
{
    public void Configure(EntityTypeBuilder<AccountBaseEntity> builder)
    {
        builder.ToTable("account_entity");
        
        builder.HasKey(account => account.Id);
        
        builder.HasMany(account => account.AccountCurrencies)
            .WithOne()
            .HasForeignKey(currency => currency.AccountId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(account => account.Transactions)
            .WithOne(transaction => transaction.Account)
            .HasForeignKey(transaction => transaction.AccountId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasDiscriminator<AccountType>("type")
            .HasValue<PersonalAccountEntity>(AccountType.Personal)
            .HasValue<UnitAccountEntity>(AccountType.Unit);
    }
}