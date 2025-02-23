using System.ComponentModel.DataAnnotations;

namespace Bank.Users.Api.Models.Profile
{
    public class ChangePasswordProfileRequest
    {
        [Required]
        public string CurrentPassword { get; set; } = null!;

        [Required]
        public string NewPassword { get; set; } = null!;
    }
}
