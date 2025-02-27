using Bank.Common.Api.Controllers;
using Bank.Common.Api.DTOs;
using Bank.Common.Auth.Attributes;
using Bank.Users.Api.Models.Profile;
using Bank.Users.Application.Users.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Users.Api.Controllers
{
    /// <summary>
    /// Отвечает за профиль текущего пользователя
    /// </summary>
    [Route("api/profile")]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    [BankAuthorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProfileController : BaseAuthController
    {
        /// <summary>
        /// Профиль текущего пользователя 
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        public Task<ActionResult> GetProfileAsync()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Обновление номера
        /// </summary>
        [HttpPut("phone")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public Task<ActionResult> ChangePhoneAsync([FromBody] ChangePhoneProfileRequest request)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Обновление пароля
        /// </summary>
        [HttpPut("password")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public Task<ActionResult> ChangePasswordAsync([FromBody] ChangePasswordProfileRequest request)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Обновление данных профиля
        /// </summary>
        [HttpPut("profile")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public Task<ActionResult> ChangeProfileAsync([FromBody] ChangeProfileRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
