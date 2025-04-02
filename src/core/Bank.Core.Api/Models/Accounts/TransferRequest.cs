namespace Bank.Core.Api.Models.Accounts;

public class TransferRequest
{
    /// <summary>
    /// Идентификатор счета отправителя
    /// </summary>
    public required long FromAccountId { get; init; }
    
    /// <summary>
    /// Идентификатор счета получателя
    /// </summary>
    public required long ToAccountId { get; init; }
    
    /// <summary>
    /// Транзакция
    /// </summary>
    public required TransactionRequest Transaction { get; init; }
}