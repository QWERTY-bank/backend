using Bank.Core.Domain.Accounts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bank.Core.Persistence.Accounts;

public class AccountCurrencyEntityConfiguration : IEntityTypeConfiguration<AccountCurrencyEntity>
{
    public void Configure(EntityTypeBuilder<AccountCurrencyEntity> builder)
    {
        builder.ToTable("account_currency_entity");

        builder.HasKey(currency => currency.Id);
    }
}