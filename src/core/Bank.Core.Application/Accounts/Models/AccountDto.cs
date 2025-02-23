using System.ComponentModel.DataAnnotations;

namespace Bank.Core.Application.Accounts.Models;

public class AccountDto
{
    /// <summary>
    ///     Идентификатор счета
    /// </summary>
    [Required]
    public required long Id { get; init; }
    
    /// <summary>
    ///     Наименование счета
    /// </summary>
    [Required]
    public required string Title { get; init; }
    
    /// <summary>
    ///     Значения валюты на счете
    /// </summary>
    [Required]
    public required IReadOnlyCollection<CurrencyValueDto> CurrencyValues { get; init; }
}