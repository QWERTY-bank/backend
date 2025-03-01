using System.ComponentModel.DataAnnotations;
using Bank.Core.Application.Accounts.Models;
using Bank.Core.Domain.Transactions;

namespace Bank.Core.Api.Models.Accounts;

public class ExtendedTransactionRequest
{
    /// <summary>
    /// Уникальный идентификатор транзакции
    /// Для идемпотентности должен заполняться на стороне клиента
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
    public required IReadOnlyCollection<CurrencyValue> CurrencyValues { get; init; }
}