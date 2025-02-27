using Bank.Users.Api.Attributes;
using Bank.Users.Domain.Users;
using System.ComponentModel.DataAnnotations;

namespace Bank.Users.Api.Models.Profile
{
    /// <summary>
    /// Запрос на обновление профиля
    /// </summary>
    public class ChangeProfileRequest
    {
        /// <summary>
        /// Новое имя профиля
        /// </summary>
        [Required]
        [MinLength(5)]
        public required string NewFullName { get; init; }

        /// <summary>
        /// Новая дата рождения
        /// </summary>
        [DataEarlierThenCurrent]
        [Required]
        public required DateOnly NewBirthday { get; init; }

        /// <summary>
        /// Новый пол
        /// </summary>
        [Required]
        public required GenderType NewGender { get; init; }
    }
}
