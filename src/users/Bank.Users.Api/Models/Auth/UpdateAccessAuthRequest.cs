using System.ComponentModel.DataAnnotations;

namespace Bank.Users.Api.Models.Auth
{
    public class UpdateAccessAuthRequest
    {
        [Required]
        public required string Refresh { get; init; }
    }
}
