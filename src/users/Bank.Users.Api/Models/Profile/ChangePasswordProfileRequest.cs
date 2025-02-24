using Bank.Users.Api.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Bank.Users.Api.Models.Profile
{
    public class ChangePasswordProfileRequest
    {
        [Required]
        public required string CurrentPassword { get; init; }

        [Required]
        [Password]
        public required string NewPassword { get; init; }
    }
}
