using System.ComponentModel.DataAnnotations;

namespace Bank.Core.Application.Accounts.Models;

public class BalanceDto
{
    /// <summary>
    ///     Идентификатор владельца счета
    /// </summary>
    [Required]
    public required Guid UserId { get; init; }
    
    /// <summary>
    ///     Закрыт ли счет
    /// </summary>
    [Required]
    public required bool IsClosed { get; init; }
    
    /// <summary>
    ///     Значения валют на счете
    /// </summary>
    [Required]
    public required IReadOnlyCollection<CurrencyValue> CurrencyValues { get; init; }
}