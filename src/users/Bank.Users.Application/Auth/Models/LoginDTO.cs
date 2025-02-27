namespace Bank.Users.Application.Auth.Models
{
    public class LoginDTO
    {
        public required string Phone { get; init; }
        public required string Password { get; init; }
    }
}
