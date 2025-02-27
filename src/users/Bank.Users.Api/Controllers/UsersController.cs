using Bank.Common.Api.Controllers;
using Bank.Common.Api.DTOs;
using Bank.Common.Auth.Attributes;
using Bank.Users.Application.Users.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using static Bank.Common.Application.Extensions.PagedListExtensions;

namespace Bank.Users.Api.Controllers
{
    /// <summary>
    /// Отвечает за пользователей
    /// </summary>
    [Route("api/users")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    [BankAuthorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UsersController : BaseAuthController
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
