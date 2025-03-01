using System.ComponentModel.DataAnnotations;
using Bank.Core.Application.Accounts.Models;

namespace Bank.Core.Api.Models.Accounts;

public class TransactionRequest
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
    public required IReadOnlyCollection<CurrencyValue> CurrencyValues { get; init; }
}