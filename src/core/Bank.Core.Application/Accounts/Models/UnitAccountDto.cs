using System.ComponentModel.DataAnnotations;

namespace Bank.Core.Application.Accounts.Models;

public class UnitAccountDto
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
    ///     Идентификатор юнита
    /// </summary>
    [Required]
    public required Guid UnitId { get; init; }
    
    /// <summary>
    ///     Значения валют на счете
    /// </summary>
    [Required]
    public required CurrencyValue CurrencyValue { get; init; }
}