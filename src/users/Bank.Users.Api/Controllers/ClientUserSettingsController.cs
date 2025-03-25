using Bank.Common.Api.Controllers;
using Bank.Common.Auth.Attributes;
using Bank.Users.Application.Settings;
using Bank.Users.Application.Settings.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Users.Api.Controllers
{
    /// <summary>
    /// Отвечает за настройки клиента
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [BankAuthorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ClientUserSettingsController : BaseController
    {
        private readonly IClientUserSettingsService _clientUserSettingsService;

        /// <summary>
        /// Конструктор
        /// </summary>
        public ClientUserSettingsController(IClientUserSettingsService clientUserSettingsService)
        {
            _clientUserSettingsService = clientUserSettingsService;
        }

        /// <summary>
        /// Получить настройки клиента
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(ClientUserSettingsDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetClientUserSettingsAsync()
        {
            return await ExecutionResultHandlerAsync(()
                => _clientUserSettingsService.GetClientUserSettingsAsync(UserId));
        }

        /// <summary>
        /// Включить/Выключить ночную тему
        /// </summary>
        /// <param name="onNightTheme">true -> включить. false -> выключить</param>
        /// <returns></returns>
        [HttpPost("theme")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> ToggleNightThemeAsync(bool onNightTheme)
        {
            return await ExecutionResultHandlerAsync(()
                => _clientUserSettingsService.ToggleNightThemeAsync(onNightTheme, UserId));
        }

        /// <summary>
        /// Показать/Скрыть счет
        /// </summary>
        /// <param name="showAccount">true -> показать. false -> скрыть</param>
        /// <param name="accountId">Номер счета</param>
        /// <returns></returns>
        [HttpPost("accounts")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> ToggleAccountAsync(bool showAccount, long accountId)
        {
            return await ExecutionResultHandlerAsync(()
                => _clientUserSettingsService.ToggleAccountAsync(showAccount, accountId, UserId));
        }
    }
}
