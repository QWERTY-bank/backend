using System.ComponentModel.DataAnnotations;
using Bank.Core.Application.Account.Models;

namespace Bank.Core.Api.Models.Accounts;

public class DepositAccountRequest
{
    /// <summary>
    ///     Транзакция начисления
    /// </summary>
    [Required]
    public required TransactionDto Transaction { get; init; }
}