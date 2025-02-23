using Bank.Users.Api.Models.Auth;
using Bank.Users.Application.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.Infrastructure.Identity.Configurations.Authorization;

namespace Bank.Users.Api.Controllers
{
    [Route("api/auth")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class AuthController : ControllerBase
    {
        /// <summary>
        /// Регистрация пользователя
        /// </summary>
        [HttpPost("registration")]
        [ProducesResponseType(typeof(TokensDTO), StatusCodes.Status200OK)]
        public async Task<ActionResult<TokensDTO>> RegistrationAsync(RegistrationAuthRequest request)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Вход
        /// </summary>
        [HttpPost("login")]
        [ProducesResponseType(typeof(TokensDTO), StatusCodes.Status200OK)]
        public async Task<ActionResult<TokensDTO>> LoginAsync(LoginAuthRequest request)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Выход
        /// </summary>
        [HttpPost("logout")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> LogoutAsync()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Обновление токена
        /// </summary>
        [HttpPost("refresh")]
        [ProducesResponseType(typeof(TokensDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize(AuthenticationSchemes = JwtBearerWithoutValidateLifetimeDefaults.CheckOnlySignature)]
        public async Task<ActionResult<TokensDTO>> UpdateAccessTokenAsync(UpdateAccessAuthRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
