using Bank.Users.Api.Attributes;
using Bank.Users.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Bank.Users.Api.Models.Profile
{
    public class ChangeProfileRequest
    {
        [Required]
        [MinLength(5)]
        public required string NewFullName { get; init; }

        [DateValidation]
        [Required]
        public required DateOnly Birthday { get; init; }

        [Required]
        public required Gender Gender { get; init; }
    }
}
