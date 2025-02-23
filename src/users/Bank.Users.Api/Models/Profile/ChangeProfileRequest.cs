using System.ComponentModel.DataAnnotations;

namespace Bank.Users.Api.Models.Profile
{
    public class ChangeProfileRequest
    {
        [Required]
        [MinLength(5)]
        public string NewFullName { get; set; } = null!;
    }
}
