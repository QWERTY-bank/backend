using Bank.Users.Api.Attributes;
using Bank.Users.Domain.Users;
using System.ComponentModel.DataAnnotations;

namespace Bank.Users.Api.Models.Auth
{
    /// <summary>
    /// Запрос на регистрацию нового пользователя
    /// </summary>
    public class RegistrationAuthRequest
    {
        /// <summary>
        /// ФИО
        /// </summary>
        [Required]
        [MinLength(5)]
        public required string FullName { get; init; }

        /// <summary>
        /// Дата рождения
        /// </summary>
        [Required]
        [DataEarlierThenCurrent]
        public required DateOnly Birthday { get; init; }

        /// <summary>
        /// Пол
        /// </summary>
        [Required]
        public required GenderType Gender { get; init; }

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
        [Password]
        public required string Password { get; init; }
    }
}
