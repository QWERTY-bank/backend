using Bank.Common.Api.Controllers;
using Bank.Common.Api.DTOs;
using Bank.Users.Api.Models.Profile;
using Bank.Users.Application.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Users.Api.Controllers
{
    [Route("api/profile")]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProfileController : BaseController
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
        /// Обновление почты
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
