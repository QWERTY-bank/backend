using System.ComponentModel.DataAnnotations;

namespace Bank.Core.Application.Accounts.Models;

public class MyBalanceDto
{
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