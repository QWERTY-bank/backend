using System.ComponentModel.DataAnnotations;

namespace Bank.Core.Api.Models.Accounts;

public class WithdrawAccountRequest
{
    /// <summary>
    ///     Транзакция списания
    /// </summary>
    [Required]
    public required TransactionRequest Transaction { get; init; }
}