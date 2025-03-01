using System.ComponentModel.DataAnnotations;
using Bank.Core.Domain.Currencies;

namespace Bank.Core.Application.Accounts.Models;

public class CurrencyValueDto
{
    /// <summary>
    ///     Код валюты (например, RUB)
    /// </summary>
    [Required]
    public required CurrencyCode Code { get; init; }
    
    /// <summary>
    ///     Значение
    /// </summary>
    [Required]
    public required decimal Value { get; init; }
}