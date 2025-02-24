using Bank.Common.Api.Controllers;
using Bank.Common.Api.DTOs;
using Bank.Users.Api.Configurations.Authorization;
using Bank.Users.Api.Models.Auth;
using Bank.Users.Application.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Users.Api.Controllers
{
    [Route("api/auth")]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public class AuthController : BaseController
    {
        /// <summary>
        /// Регистрация пользователя
        /// </summary>
        [HttpPost("registration")]
        [ProducesResponseType(typeof(TokensDTO), StatusCodes.Status200OK)]
        public Task<ActionResult<TokensDTO>> RegistrationAsync(RegistrationAuthRequest request)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Вход
        /// </summary>
        [HttpPost("login")]
        [ProducesResponseType(typeof(TokensDTO), StatusCodes.Status200OK)]
        public Task<ActionResult<TokensDTO>> LoginAsync(LoginAuthRequest request)
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
        public Task<ActionResult> LogoutAsync()
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
        public Task<ActionResult<TokensDTO>> UpdateAccessTokenAsync(UpdateAccessAuthRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
