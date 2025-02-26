namespace Bank.Core.Domain.Transactions;

public class DepositTransaction : TransactionEntity
{
    public DepositTransaction()
    {
        Type = TransactionType.Deposit;
    }
}