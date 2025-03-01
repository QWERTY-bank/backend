using System.ComponentModel.DataAnnotations;
using Bank.Core.Application.Accounts.Models;

namespace Bank.Core.Api.Models.Accounts;

public class WithdrawAccountRequest
{
    /// <summary>
    ///     Транзакция списания
    /// </summary>
    [Required]
    public required TransactionRequest Transaction { get; init; }
}