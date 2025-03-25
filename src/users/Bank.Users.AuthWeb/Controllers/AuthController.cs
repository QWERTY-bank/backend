using Bank.Users.Application.Auth;
using Bank.Users.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Users.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [Route("auth/login")]
        [HttpGet]
        public IActionResult Login(string redirectUrl)
        {
            var model = new LoginViewModel()
            {
                ReturnUrl = redirectUrl
            };
            return View(model);
        }

        [Route("auth/login")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _authService.LoginAsync(new() { Password = model.Password, Phone = model.Phone });

            if (result.IsNotSuccess)
            {
                if (result.Errors.ContainsKey("LoginFail"))
                {
                    ModelState.AddModelError(string.Empty, "Неверный номер телефона или пароль");
                }
                else if (result.Errors.ContainsKey("UserBlocked"))
                {
                    ModelState.AddModelError(string.Empty, "Ваш аккаунт заблокирован");
                }

                return View(model);
            }

            return Redirect($"{model.ReturnUrl}?access={result.Result.Access}&refresh={result.Result.Refresh}");
        }
    }
}
