using System.ComponentModel.DataAnnotations;

namespace Bank.Users.Api.Models.Auth
{
    /// <summary>
    /// Запрос на вход с помощью кода
    /// </summary>
    public class CodeLoginAuthRequest
    {
        /// <summary>
        /// Код
        /// </summary>
        [Required]
        public required string Code { get; init; }
    }
}
