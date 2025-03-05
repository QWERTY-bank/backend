using Bank.Common.Api.Controllers;
using Bank.Common.Api.DTOs;
using Bank.Users.Application.Auth;
using Bank.Users.Application.Auth.Models;
using Microsoft.AspNetCore.Mvc;
using Z1all.ExecutionResult.StatusCode;

namespace Bank.Users.Api.Controllers
{
    /// <summary>
    /// Отвечает за аутентификацию сервисов
    /// </summary>
    [Route("api/service/auth")]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public class ServiceAuthController : BaseController
    {
        private readonly IServiceTokenService _serviceTokenService;

        /// <summary>
        /// Конструктор
        /// </summary>
        public ServiceAuthController(IServiceTokenService serviceTokenService)
        {
            _serviceTokenService = serviceTokenService;
        }

        /// <summary>
        /// Создает токены сервисов для back to back взаимодействия
        /// </summary>
        [HttpGet("login")]
        [ProducesResponseType(typeof(ServiceTokenDto), StatusCodes.Status200OK)]
        public IActionResult ServiceLogin()
        {
            var result = _serviceTokenService.CreateServiceTokens();

            // TODO: Добавить проверку пароля

            if (!result.IsSuccess) return ExecutionResultHandler(ExecutionResult.FromError(result));
            return Ok(result.Result!);
        }
    }
}
