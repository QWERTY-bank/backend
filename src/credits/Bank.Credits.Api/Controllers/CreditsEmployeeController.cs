using Bank.Common.Api.Controllers;
using Bank.Common.Api.DTOs;
using Bank.Common.Auth.Attributes;
using Bank.Credits.Api.Models.Credits;
using Bank.Credits.Application.Credits;
using Microsoft.AspNetCore.Mvc;
using static Bank.Common.Application.Extensions.PagedListExtensions;

namespace Bank.Credits.Api.Controllers
{
    /// <summary>
    /// Отвечает за кредиты
    /// </summary>
    [Route("api/credits/employee")]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    [BankAuthorize]
    public class CreditsEmployeeController : BaseController
    {
        /// <summary>
        /// Получить историю кредитов пользователя (для сотрудника)
        /// </summary>
        [HttpGet("user/{userId}")]
        [ProducesResponseType(typeof(PagedListWithMetadata<CreditShortDto>), StatusCodes.Status200OK)]
        public Task<ActionResult> GetCreditsAsync([FromQuery] CreditsFilter filter, [FromQuery] Pagination pagination, [FromRoute] Guid userId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Получить информацию по кредиту пользователя (для сотрудника)
        /// </summary>
        [HttpGet("{creditId}/user/{userId}")]
        [ProducesResponseType(typeof(CreditDto), StatusCodes.Status200OK)]
        public Task<ActionResult> GetCreditAsync([FromRoute] Guid creditId, [FromRoute] Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
