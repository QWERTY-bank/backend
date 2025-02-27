using Bank.Common.Api.Controllers;
using Bank.Common.Api.DTOs;
using Bank.Credits.Api.Models.Credits;
using Bank.Credits.Application.Credits;
using Microsoft.AspNetCore.Mvc;
using static Bank.Common.Application.Extensions.PagedListExtensions;

namespace Bank.Credits.Api.Controllers
{
    /// <summary>
    /// Отвечает за кредиты
    /// </summary>
    [Route("api/credits")]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public class CreditsController : BaseAuthController
    {
        /// <summary>
        /// Получить историю кредитов
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(PagedListWithMetadata<CreditShortDto>), StatusCodes.Status200OK)]
        public Task<ActionResult> GetCreditsAsync([FromQuery] CreditsFilter filter, [FromQuery] Pagination pagination)
        {
            throw new NotImplementedException();
        }

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
        /// Получить информацию по кредиту
        /// </summary>
        [HttpGet("{creditId}")]
        [ProducesResponseType(typeof(CreditDto), StatusCodes.Status200OK)]
        public Task<ActionResult> GetCreditAsync([FromRoute] Guid creditId)
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

        /// <summary>
        /// Взять кредит
        /// </summary>
        [HttpPost("apply")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public Task<ActionResult> TakeCreditAsync([FromBody] TakeCreditRequest request)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Уменьшить сумму долга по кредиту
        /// </summary>
        [HttpPost("{creditId}/reduce_payment")]
        public Task<ActionResult> ReduceCreditAsync([FromBody] ReduceCreditRequest request, [FromRoute] Guid creditId)
        {
            throw new NotImplementedException();
        }
    }
}
