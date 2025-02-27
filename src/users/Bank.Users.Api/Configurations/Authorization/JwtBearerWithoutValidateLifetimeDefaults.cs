namespace Bank.Users.Api.Configurations.Authorization
{
    /// <summary>
    /// Ключи для схемы авторизации без валидации времени жизни jwt токена
    /// </summary>
    public static class JwtBearerWithoutValidateLifetimeDefaults
    {
        /// <summary>
        /// Ключ схемы авторизации для проверки только сигнатуры токена
        /// </summary>
        public const string CheckOnlySignature = "CheckOnlySignature";
    }
}
