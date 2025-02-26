using Bank.Common.Api.Controllers;
using Bank.Common.Api.DTOs;
using Bank.Users.Application.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Bank.Common.Application.Extensions.PagedListExtensions;

namespace Bank.Users.Api.Controllers
{
    [Route("api/users")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UsersController : BaseController
    {
        /// <summary>
        /// Получить пользователей
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(PagedListWithMetadata<UserShortDto>), StatusCodes.Status200OK)]
        public Task<ActionResult> GetUsersAsync([FromQuery] Pagination pagination)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Получить профиль пользователя
        /// </summary>
        [HttpGet("{userId}")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        public Task<ActionResult> GetUserAsync([FromRoute] Guid userId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Заблокировать пользователя (для сотрудников)
        /// </summary>
        [HttpPost("{userId}/block")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public Task<ActionResult> BlockUserAsync([FromRoute] Guid userId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Разблокировать пользователя (для сотрудников)
        /// </summary>
        [HttpPost("{userId}/unblock")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public Task<ActionResult> UnblockUserAsync([FromRoute] Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
