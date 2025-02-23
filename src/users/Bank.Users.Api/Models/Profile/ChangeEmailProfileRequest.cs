using System.ComponentModel.DataAnnotations;

namespace Bank.Users.Api.Models.Profile
{
    public class ChangeEmailProfileRequest
    {
        [Required]
        [EmailAddress]
        public string NewEmail { get; set; } = null!;
    }
}
