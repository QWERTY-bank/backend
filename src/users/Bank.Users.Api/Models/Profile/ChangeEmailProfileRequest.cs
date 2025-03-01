using Bank.Users.Api.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Bank.Users.Api.Models.Profile
{
    /// <summary>
    /// Запрос на смену телефона
    /// </summary>
    public class ChangePhoneProfileRequest
    {
        /// <summary>
        /// Новый номер телефона
        /// </summary>
        [Required]
        [RuPhoneNumber]
        public required string NewPhone { get; init; }
    }
}
