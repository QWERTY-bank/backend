using Bank.Core.Domain.Accounts;
using Bank.Core.Domain.Currencies;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bank.Core.Persistence.Currencies;

public class CurrencyEntityConfiguration : IEntityTypeConfiguration<CurrencyEntity>
{
    public void Configure(EntityTypeBuilder<CurrencyEntity> builder)
    {
        builder.ToTable("currency_entity");

        builder.HasKey(currency => currency.Id);
        
        builder.Property(currency => currency.Code)
            .HasConversion<string>();

        builder.HasMany<AccountCurrencyEntity>()
            .WithOne()
            .HasForeignKey(resource => resource.Code)
            .HasPrincipalKey(resource => resource.Code)
            .OnDelete(DeleteBehavior.Restrict);
    }
}