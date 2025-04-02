using System.ComponentModel.DataAnnotations;
using Bank.Core.Domain.Currencies;

namespace Bank.Core.Api.Models.Accounts;

/// <summary>
///     Запрос на создание счета
/// </summary>
public class CreateAccountRequest
{
    /// <summary>
    ///     Наименование счета
    /// </summary>
    [Required(AllowEmptyStrings = false, ErrorMessage = "Наименование счета не может быть пустым")]
    [MaxLength(300, ErrorMessage = "Наименование счета не может быть длиннее {1} символов")]
    public required string Title { get; init; }
    
    /// <summary>
    ///     Код валюты
    /// </summary>
    [Required]
    public required CurrencyCode Code { get; init; }
}