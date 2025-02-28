using Bank.Common.Api.Controllers;
using Bank.Common.Api.DTOs;
using Bank.Common.Auth.Attributes;
using Bank.Users.Application.Users;
using Bank.Users.Application.Users.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;
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
    public class UsersController : BaseController
    {
        private readonly IUserService _userService;

        /// <summary>
        /// Конструктор
        /// </summary>
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Получить пользователей
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(PagedListWithMetadata<UserShortDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IPagedList<UserShortDto>>> GetUsersAsync([FromQuery] Pagination pagination)
        {
            return await ExecutionResultHandlerAsync(()
                => _userService.GetUsersAsync(pagination.Page, pagination.Size));
        }

        /// <summary>
        /// Получить профиль пользователя
        /// </summary>
        [HttpGet("{userId}")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<UserDto>> GetUserAsync([FromRoute] Guid userId)
        {
            return await ExecutionResultHandlerAsync(()
                => _userService.GetUserAsync(userId));
        }

        /// <summary>
        /// Заблокировать пользователя (для сотрудников)
        /// </summary>
        [HttpPost("{userId}/block")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> BlockUserAsync([FromRoute] Guid userId)
        {
            return await ExecutionResultHandlerAsync(()
                => _userService.ChangeUserBlockStatusAsync(true, userId));
        }

        /// <summary>
        /// Разблокировать пользователя (для сотрудников)
        /// </summary>
        [HttpPost("{userId}/unblock")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> UnblockUserAsync([FromRoute] Guid userId)
        {
            return await ExecutionResultHandlerAsync(()
                => _userService.ChangeUserBlockStatusAsync(false, userId));
        }
    }
}
