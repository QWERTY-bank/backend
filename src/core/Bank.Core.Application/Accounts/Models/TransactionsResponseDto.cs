using System.ComponentModel.DataAnnotations;

namespace Bank.Core.Application.Accounts.Models;

public class TransactionsResponseDto
{
    /// <summary>
    /// Транзакции
    /// </summary>
    [Required]
    public required IReadOnlyCollection<TransactionDto> Transactions { get; init; }
    
    /// <summary>
    /// Доходы
    /// </summary>
    [Required]
    public required CurrencyValue DepositCurrencyTotals { get; init; }
    
    /// <summary>
    /// Траты
    /// </summary>
    [Required]
    public required CurrencyValue WithdrawCurrencyTotals { get; init; }
}