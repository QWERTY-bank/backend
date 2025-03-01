using Bank.Core.Domain.Accounts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bank.Core.Persistence.Accounts;

public class PersonalAccountEntityTypeConfiguration : IEntityTypeConfiguration<PersonalAccountEntity>
{
    public void Configure(EntityTypeBuilder<PersonalAccountEntity> builder)
    {
        builder.HasIndex(account => new { account.UserId, account.Title }).IsUnique();
    }
}