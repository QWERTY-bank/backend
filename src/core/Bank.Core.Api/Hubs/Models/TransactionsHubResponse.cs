using System.ComponentModel.DataAnnotations;
using Bank.Core.Application.Accounts.Models;

namespace Bank.Core.Api.Hubs.Models;

public class TransactionsHubResponse
{
    [Required]
    public required CurrencyValue CurrencyValue { get; init; }
    
    [Required]
    public required IReadOnlyCollection<TransactionDto> Transactions { get; init; }
}