namespace Bank.Users.Application.Auth.Models
{
    public class ServiceTokenDto
    {
        public string Token { get; set; } = null!;
        public DateTime ExpiredDateTime { get; set; }
    }
}
