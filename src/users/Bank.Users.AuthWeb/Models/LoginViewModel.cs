using System.ComponentModel.DataAnnotations;

namespace Bank.Users.Web.Models
{
    public class LoginViewModel
    {
        [Required]
        public string Phone { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string ReturnUrl { get; set; }
    }
}
