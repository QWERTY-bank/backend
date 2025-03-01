namespace Bank.Core.Domain.Transactions;

/// <summary>
/// Тип транзакции
/// </summary>
public enum TransactionType
{
    /// <summary>
    /// Начисление
    /// </summary>
    Deposit = 0,
    
    /// <summary>
    /// Списание
    /// </summary>
    Withdraw = 1,
}
