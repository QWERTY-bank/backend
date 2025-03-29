using Bank.Common.Api.Controllers;
using Bank.Credits.Application.User;
using Bank.Credits.Application.User.Models;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Credits.Api.Controllers
{
    /// <summary>
    /// Отвечает за информацию о пользователе
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;

        /// <summary>
        /// Конструктор
        /// </summary>
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Получить информацию о пользователе
        /// </summary>
        [HttpGet("{userId}")]
        [ProducesResponseType(typeof(UserCreditInfoDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserCreditInfoAsync(Guid userId)
        {
            return await ExecutionResultHandlerAsync(() 
                    => _userService.GetUserCreditInfoAsync(userId));
        }
    }
}
