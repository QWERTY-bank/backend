using System.ComponentModel.DataAnnotations;

namespace Bank.Core.Application.Accounts.Models;

public class TransactionDto
{
    /// <summary>
    /// Уникальный идентификатор транзакции
    /// Для идемпотентности должен заполняться на стороне клиента
    /// </summary>
    [Required]
    public required Guid Key { get; init; }
    
    /// <summary>
    /// Операции начисления и списания валют
    /// </summary>
    [Required]
    public required IReadOnlyCollection<CurrencyValueDto> CurrencyValues { get; init; }
}