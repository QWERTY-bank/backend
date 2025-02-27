namespace Bank.Users.Application.Auth.Models
{
    public class TokensDTO
    {
        public required string Refresh { get; set; }
        public required DateTime RefreshExpiredDateTime { get; set; }

        public required string Access { get; set; }
        public required DateTime AccessExpiredDateTime { get; set; }
    }
}
