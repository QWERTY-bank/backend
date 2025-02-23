using Bank.Users.Api.Models.Profile;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Users.Api.Controllers
{
    [Route("api/user")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProfileController : ControllerBase
    {
        /// <summary>
        /// Обновление почты
        /// </summary>
        [HttpPatch("email")]
        public async Task<ActionResult> ChangeEmailAsync([FromBody] ChangeEmailProfileRequest request)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Обновление пароля
        /// </summary>
        [HttpPatch("password")]
        public async Task<ActionResult> ChangePasswordAsync([FromBody] ChangePasswordProfileRequest request)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Обновление данных профиля
        /// </summary>
        [HttpPatch("profile")]
        public async Task<ActionResult> ChangeProfileAsync([FromBody] ChangeProfileRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
