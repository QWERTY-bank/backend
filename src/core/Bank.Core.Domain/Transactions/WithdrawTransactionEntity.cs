namespace Bank.Core.Domain.Transactions;

public class WithdrawTransactionEntity : TransactionEntity
{
    public WithdrawTransactionEntity()
    {
        Type = TransactionType.Withdraw;
    }
}