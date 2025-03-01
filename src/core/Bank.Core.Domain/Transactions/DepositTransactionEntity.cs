namespace Bank.Core.Domain.Transactions;

public class DepositTransactionEntity : TransactionEntity
{
    public DepositTransactionEntity()
    {
        Type = TransactionType.Deposit;
    }
}