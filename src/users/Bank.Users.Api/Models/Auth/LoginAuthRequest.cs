using Bank.Users.Api.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Bank.Users.Api.Models.Auth
{
    public class LoginAuthRequest
    {
        [Required]
        [PhoneNumber]
        public required string Phone { get; init; }

        [Required]
        public required string Password { get; init; }
    }
}
