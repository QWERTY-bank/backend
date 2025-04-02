using Bank.Core.Domain.Accounts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bank.Core.Persistence.Accounts;

public class UnitAccountEntityTypeConfiguration : IEntityTypeConfiguration<UnitAccountEntity>
{
    public void Configure(EntityTypeBuilder<UnitAccountEntity> builder)
    {
        builder.HasIndex(account => account.UnitId);
    }
}