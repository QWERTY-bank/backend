namespace Bank.Users.Application.Auth.Configurations
{
    public class JwtServiceOptions
    {
        public string SecretKey { get; set; } = null!;
        public int AccessTokenTimeLifeMinutes { get; set; }
    }
}
