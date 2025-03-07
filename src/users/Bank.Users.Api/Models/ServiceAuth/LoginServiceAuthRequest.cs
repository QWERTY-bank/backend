namespace Bank.Users.Api.Models.ServiceAuth
{
    /// <summary>
    /// Запрос на получение токена сервисов
    /// </summary>
    public class LoginServiceAuthRequest
    {
        /// <summary>
        /// Секрет
        /// </summary>
        public required string Secret { get; init; }
    }
}
