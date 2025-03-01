using System.ComponentModel.DataAnnotations;

namespace Bank.Users.Api.Models.Auth
{
    /// <summary>
    /// Запрос на обновление токена
    /// </summary>
    public class UpdateAccessAuthRequest
    {
        /// <summary>
        /// Токен обновления
        /// </summary>
        [Required]
        public required string Refresh { get; init; }
    }
}
