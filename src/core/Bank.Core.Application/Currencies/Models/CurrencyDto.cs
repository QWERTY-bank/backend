using System.ComponentModel.DataAnnotations;

namespace Bank.Core.Application.Currencies.Models;

public class CurrencyDto
{
    /// <summary>
    ///     Код валюты (например, RUB)
    /// </summary>
    [Required]
    public required string Code { get; init; }
}