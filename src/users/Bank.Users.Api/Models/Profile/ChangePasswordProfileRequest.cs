using Bank.Users.Api.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Bank.Users.Api.Models.Profile
{
    /// <summary>
    /// Запрос на обновление пароля
    /// </summary>
    public class ChangePasswordProfileRequest
    {
        /// <summary>
        /// Текущий пароль
        /// </summary>
        [Required]
        public required string CurrentPassword { get; init; }

        /// <summary>
        /// Новый пароль
        /// </summary>
        [Required]
        [Password]
        public required string NewPassword { get; init; }
    }
}
