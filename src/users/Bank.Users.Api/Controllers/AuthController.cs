using AutoMapper;
using Bank.Common.Api.Controllers;
using Bank.Common.Api.DTOs;
using Bank.Common.Auth.Attributes;
using Bank.Users.Api.Configurations.Authorization;
using Bank.Users.Api.Models.Auth;
using Bank.Users.Application.Auth;
using Bank.Users.Application.Auth.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;

// + Добавить хэширование пароля
// + Связять контроллер auth и сервис auth
// + Обновить dockerfile
// + Добавить конфигурации в appsettings
// Добавить миграцию
// + Добавить postgres и redis в docker compose

namespace Bank.Users.Api.Controllers
{
    /// <summary>
    /// Отвечает за регистрация и аутентификация 
    /// </summary>
    [Route("api/auth")]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public class AuthController : BaseController
    {
        private readonly IAuthService _authService;
        private readonly ITokensService _tokensService;
        private readonly IMapper _mapper;

        /// <summary>
        /// Конструктор
        /// </summary>
        public AuthController(
            IAuthService authService,
            ITokensService tokensService,
            IMapper mapper)
        {
            _authService = authService;
            _tokensService = tokensService;
            _mapper = mapper;
        }

        /// <summary>
        /// Регистрация пользователя
        /// </summary>
        [HttpPost("registration")]
        [ProducesResponseType(typeof(TokensDTO), StatusCodes.Status200OK)]
        public async Task<ActionResult<TokensDTO>> RegistrationAsync(RegistrationAuthRequest request)
        {
            return await ExecutionResultHandlerAsync(() 
                => _authService.RegistrationAsync(_mapper.Map<RegistrationDTO>(request)));
        }

        /// <summary>
        /// Вход
        /// </summary>
        [HttpPost("login")]
        [ProducesResponseType(typeof(TokensDTO), StatusCodes.Status200OK)]
        public async Task<ActionResult<TokensDTO>> LoginAsync(LoginAuthRequest request)
        {
            return await ExecutionResultHandlerAsync(()
                => _authService.LoginAsync(_mapper.Map<LoginDTO>(request)));
        }

        /// <summary>
        /// Выход
        /// </summary>
        [HttpPost("logout")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [BankAuthorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> LogoutAsync()
        {
            return await ExecutionResultHandlerAsync(()
                => _tokensService.RemoveRefreshAsync(TokenJTI));
        }

        /// <summary>
        /// Обновление токена
        /// </summary>
        [HttpPost("refresh")]
        [ProducesResponseType(typeof(TokensDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [BankAuthorize(AuthenticationSchemes = JwtBearerWithoutValidateLifetimeDefaults.CheckOnlySignature)]
        public async Task<ActionResult<TokensDTO>> UpdateAccessTokenAsync(UpdateAccessAuthRequest request)
        {
            return await ExecutionResultHandlerAsync(()
                => _authService.UpdateAccessTokenAsync(request.Refresh, TokenJTI, UserId));
        }
    }
}
