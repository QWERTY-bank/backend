using Bank.Users.Application.Auth;
using Bank.Users.AuthWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Users.AuthWeb.Controllers
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

            var result = await _authService.GetLoginCodeAsync(new() { Password = model.Password, Phone = model.Phone });

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

            return Redirect($"{model.ReturnUrl}?code={result.Result.Code}");
        }
    }
}
