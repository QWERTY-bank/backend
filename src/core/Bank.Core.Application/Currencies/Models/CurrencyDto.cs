using System.ComponentModel.DataAnnotations;
using Bank.Core.Domain.Currencies;

namespace Bank.Core.Application.Currencies.Models;

public class CurrencyDto
{
    /// <summary>
    ///     Код валюты (например, RUB)
    /// </summary>
    [Required]
    public required CurrencyCode Code { get; init; }
}