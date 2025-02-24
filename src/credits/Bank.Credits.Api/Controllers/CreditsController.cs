using Bank.Common.Api.Controllers;
using Bank.Common.Api.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Credits.Api.Controllers
{
    [Route("api/credits")]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    [Authorize]
    public class CreditsController : BaseController
    {
        /// <summary>
        /// Получить историю кредитов
        /// </summary>
        [HttpGet]
        public Task<ActionResult> GetCreditsAsync()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Получить историю кредитов пользователя (для сотрудника)
        /// </summary>
        [HttpGet("user/{userId}")]
        public Task<ActionResult> GetCreditsAsync([FromRoute] Guid userId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Взять кредит
        /// </summary>
        [HttpPost("apply")]
        public Task<ActionResult> TakeCreditAsync(Guid tarifId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Получить информацию по кредиту
        /// </summary>
        [HttpGet("{creditId}")]
        public Task<ActionResult> GetCreditAsync([FromRoute] Guid creditId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Получить информацию по кредиту пользователя (для сотрудника)
        /// </summary>
        [HttpGet("{creditId}/user/{userId}")]
        public Task<ActionResult> GetCreditAsync([FromRoute] Guid creditId, [FromRoute] Guid userId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Уменьшить сумму долга по кредиту
        /// </summary>
        [HttpPost("{creditId}/reduce_payment")]
        public Task<ActionResult> ReduceCreditAsync([FromRoute] Guid creditId)
        {
            throw new NotImplementedException();
        }
    }
}
