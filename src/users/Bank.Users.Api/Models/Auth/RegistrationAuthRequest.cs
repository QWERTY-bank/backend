using System.ComponentModel.DataAnnotations;

namespace Bank.Users.Api.Models.Auth
{
    public class RegistrationAuthRequest
    {
        [Required]
        [MinLength(5)]
        public string FullName { get; init; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; init; } = null!;

        [Required]
        public string Password { get; init; } = null!;
    }
}
