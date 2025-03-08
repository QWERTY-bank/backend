namespace Bank.Common.Auth.Configurations
{
    public class JwtOptions
    {
        public string SecretKey { get; set; } = null!;
        public TimeSpan AccessTokenTimeLife { get; set; }
        public int RefreshTokenTimeLifeDays { get; set; }
    }
}
