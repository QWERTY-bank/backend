using Bank.Common.Api.Controllers;
using Bank.Common.Api.DTOs;
using Bank.Common.Application.Models;
using Bank.Common.Application.Z1all.ExecutionResult.StatusCode;
using Bank.Users.Api.Models.ServiceAuth;
using Bank.Users.Application.Auth;
using Microsoft.AspNetCore.Mvc;

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
        private readonly string _secret;

        /// <summary>
        /// Конструктор
        /// </summary>
        public ServiceAuthController(IServiceTokenService serviceTokenService, IConfiguration configuration)
        {
            _serviceTokenService = serviceTokenService;
            _secret = configuration.GetRequiredSection("TokenService:Secret").Value!;
        }

        /// <summary>
        /// Создает токены сервисов для back to back взаимодействия
        /// </summary>
        [HttpPost("login")]
        [ProducesResponseType(typeof(ServiceTokenDto), StatusCodes.Status200OK)]
        public IActionResult ServiceLogin(LoginServiceAuthRequest request)
        {
            if (request.Secret != _secret)
            {
                return StatusCode(StatusCodes.Status403Forbidden);
            }

            var result = _serviceTokenService.CreateServiceTokens();

            if (!result.IsSuccess) return ExecutionResultHandler(ExecutionResult.FromError(result));
            return Ok(result.Result!);
        }
    }
}
