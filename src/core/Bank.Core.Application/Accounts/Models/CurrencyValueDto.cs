using System.ComponentModel.DataAnnotations;

namespace Bank.Core.Application.Accounts.Models;

public class CurrencyValueDto
{
    /// <summary>
    ///     Код валюты (например, RUB)
    /// </summary>
    [Required]
    public required string Code { get; init; }
    
    /// <summary>
    ///     Значение
    /// </summary>
    [Required]
    public required decimal Value { get; init; }
}