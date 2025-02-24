namespace Bank.Users.Application.Auth
{
    public class TokensDTO
    {
        public required string Refresh { get; set; }
        public required string Access { get; set; }
    }
}
