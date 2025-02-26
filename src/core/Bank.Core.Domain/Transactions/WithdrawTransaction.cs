namespace Bank.Core.Domain.Transactions;

public class WithdrawTransaction : TransactionEntity
{
    public WithdrawTransaction()
    {
        Type = TransactionType.Withdraw;
    }
}