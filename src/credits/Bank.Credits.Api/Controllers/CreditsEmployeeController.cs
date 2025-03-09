using AutoMapper;
using Bank.Common.Api.Controllers;
using Bank.Common.Api.DTOs;
using Bank.Common.Auth.Attributes;
using Bank.Common.Models.Auth;
using Bank.Credits.Application.Credits;
using Bank.Credits.Application.Credits.Models;
using Microsoft.AspNetCore.Mvc;
using static Bank.Common.Application.Extensions.PagedListExtensions;

namespace Bank.Credits.Api.Controllers
{
    /// <summary>
    /// Отвечает за кредиты
    /// </summary>
    [Route("api/employee/credits")]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    [BankAuthorize(RoleType.Employee)]
    public class CreditsEmployeeController : BaseController
    {
        private readonly ICreditsService _creditsService;
        private readonly IMapper _mapper;

        /// <summary>
        /// Конструктор
        /// </summary>
        public CreditsEmployeeController(
            ICreditsService creditsService,
            IMapper mapper)
        {
            _creditsService = creditsService;
            _mapper = mapper;
        }

        /// <summary>
        /// Получить историю кредитов пользователя (для сотрудника)
        /// </summary>
        [HttpGet("user/{userId}")]
        [ProducesResponseType(typeof(PagedListWithMetadata<CreditShortDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCreditsAsync([FromQuery] CreditsFilter filter, [FromQuery] Pagination pagination, [FromRoute] Guid userId)
        {
            return await ExecutionResultHandlerAsync(()
                => _creditsService.GetCreditsAsync(filter, pagination.Page, pagination.Size, userId));
        }

        /// <summary>
        /// Получить информацию по кредиту пользователя (для сотрудника)
        /// </summary>
        [HttpGet("{creditId}/user/{userId}")]
        [ProducesResponseType(typeof(CreditDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCreditAsync([FromRoute] Guid creditId, [FromRoute] Guid userId)
        {
            return await ExecutionResultHandlerAsync(()
                => _creditsService.GetCreditAsync(creditId, userId));
        }
    }
}
