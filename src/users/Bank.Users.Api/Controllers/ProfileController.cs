using AutoMapper;
using Bank.Common.Api.Controllers;
using Bank.Common.Api.DTOs;
using Bank.Common.Auth.Attributes;
using Bank.Users.Api.Models.Profile;
using Bank.Users.Application.Users;
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
    public class ProfileController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        /// <summary>
        /// Конструктор
        /// </summary>
        public ProfileController(
            IUserService userService, 
            IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        /// <summary>
        /// Профиль текущего пользователя 
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProfileAsync()
        {
            return await ExecutionResultHandlerAsync(() 
                => _userService.GetUserAsync(UserId));
        }

        /// <summary>
        /// Обновление номера
        /// </summary>
        [HttpPut("phone")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> ChangePhoneAsync([FromBody] ChangePhoneProfileRequest request)
        {
            return await ExecutionResultHandlerAsync(()
                => _userService.ChangePhoneAsync(_mapper.Map<ChangePhoneDto>(request), UserId));
        }

        /// <summary>
        /// Обновление пароля
        /// </summary>
        [HttpPut("password")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> ChangePasswordAsync([FromBody] ChangePasswordProfileRequest request)
        {
            return await ExecutionResultHandlerAsync(()
                => _userService.ChangePasswordAsync(_mapper.Map<ChangePasswordDto>(request), UserId));
        }

        /// <summary>
        /// Обновление данных профиля
        /// </summary>
        [HttpPut("profile")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> ChangeProfileAsync([FromBody] ChangeProfileRequest request)
        {
            return await ExecutionResultHandlerAsync(()
                => _userService.ChangeProfileAsync(_mapper.Map<ChangeProfileDto>(request), UserId));
        }
    }
}
