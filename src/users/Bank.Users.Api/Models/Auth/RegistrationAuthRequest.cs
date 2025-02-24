using Bank.Users.Api.Attributes;
using Bank.Users.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Bank.Users.Api.Models.Auth
{
    public class RegistrationAuthRequest
    {
        [Required]
        [MinLength(5)]
        public required string FullName { get; init; }

        [DateValidation]
        [Required]
        public required DateOnly Birthday { get; init; }

        [Required]
        public required Gender Gender { get; init; }

        [Required]
        [PhoneNumber]
        public required string Phone { get; init; }

        [Required]
        [Password]
        public required string Password { get; init; }
    }
}
