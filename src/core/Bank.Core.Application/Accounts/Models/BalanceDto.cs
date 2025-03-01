using System.ComponentModel.DataAnnotations;

namespace Bank.Core.Application.Accounts.Models;

public class BalanceDto
{
    /// <summary>
    ///     Значения валют на счете
    /// </summary>
    [Required]
    public required IReadOnlyCollection<CurrencyValueDto> CurrencyValues { get; init; }
}