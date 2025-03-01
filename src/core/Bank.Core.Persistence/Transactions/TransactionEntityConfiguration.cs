using Bank.Core.Domain.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bank.Core.Persistence.Transactions;

public class TransactionEntityConfiguration : IEntityTypeConfiguration<TransactionEntity>
{
    public void Configure(EntityTypeBuilder<TransactionEntity> builder)
    {
        builder.ToTable("transaction_entity");

        builder.HasKey(transaction => transaction.Id);
        
        builder.Property(transaction => transaction.CreatedDate).HasDefaultValueSql("now()");
        builder.Property(transaction => transaction.Currencies).HasColumnType("jsonb");
        
        builder.HasDiscriminator(transaction => transaction.Type)
            .HasValue<DepositTransactionEntity>(TransactionType.Deposit)
            .HasValue<WithdrawTransactionEntity>(TransactionType.Withdraw);
        
        builder.HasMany(transaction => transaction.ChildTransactions)
            .WithOne()
            .HasForeignKey(transaction => transaction.ParentKey)
            .HasPrincipalKey(transaction => transaction.Key)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasIndex(transaction => new {transaction.AccountId, transaction.OperationDate});
    }
}