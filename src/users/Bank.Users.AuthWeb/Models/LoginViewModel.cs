using System.ComponentModel.DataAnnotations;

namespace Bank.Users.AuthWeb.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Обязательное поле")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        [Required(ErrorMessage = "Значение ReturnUrl не установлено")]
        public string ReturnUrl { get; set; }
    }
}
