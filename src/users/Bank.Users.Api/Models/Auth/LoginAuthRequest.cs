using Bank.Users.Api.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Bank.Users.Api.Models.Auth
{
    /// <summary>
    /// Запрос на вход
    /// </summary>
    public class LoginAuthRequest
    {
        /// <summary>
        /// Номер телефона
        /// </summary>
        [Required]
        [RuPhoneNumber]
        public required string Phone { get; init; }

        /// <summary>
        /// Пароль
        /// </summary>
        [Required]
        public required string Password { get; init; }
    }
}
