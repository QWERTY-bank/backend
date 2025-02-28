namespace Bank.Users.Application.Users.Models
{
    public class ChangePasswordDto
    {
        public required string CurrentPassword { get; init; }
        public required string NewPassword { get; init; }
    }
}
