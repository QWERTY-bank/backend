using System.ComponentModel.DataAnnotations;

namespace Bank.Core.Application.Accounts.Models;

public class AdminAccountDto
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
    ///     Идентификатор пользователя
    /// </summary>
    [Required]
    public required Guid? UserId { get; init; }
    
    /// <summary>
    ///     Закрыт ли счет
    /// </summary>
    [Required]
    public required bool IsClosed { get; init; }
    
    /// <summary>
    ///     Значения валют на счете
    /// </summary>
    [Required]
    public required CurrencyValue CurrencyValue { get; init; }
    
    /// <summary>
    ///     Мастер ли счет
    /// </summary>
    [Required]
    public required bool IsMaster { get; init; }
}