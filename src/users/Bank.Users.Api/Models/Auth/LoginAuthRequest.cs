using System.ComponentModel.DataAnnotations;

namespace Bank.Users.Api.Models.Auth
{
    public class LoginAuthRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; init; } = null!;

        [Required]
        public string Password { get; init; } = null!;
    }
}
