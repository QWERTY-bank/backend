using Bank.Users.Api.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Bank.Users.Api.Models.Profile
{
    public class ChangePhoneProfileRequest
    {
        [Required]
        [PhoneNumber]
        public required string NewPhone { get; init; }
    }
}
