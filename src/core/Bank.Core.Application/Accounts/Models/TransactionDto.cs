using System.ComponentModel.DataAnnotations;
using Bank.Core.Domain.Transactions;

namespace Bank.Core.Application.Accounts.Models;

public class TransactionDto
{
    /// <summary>
    /// Уникальный идентификатор транзакции
    /// </summary>
    [Required]
    public required Guid Key { get; init; }
    
    /// <summary>
    /// Тип транзакции
    /// </summary>
    [Required]
    public required TransactionType Type { get; init; }
    
    /// <summary>
    /// Операции начисления и списания валют
    /// </summary>
    [Required]
    public required IReadOnlyCollection<CurrencyValueDto> CurrencyValues { get; init; }
}