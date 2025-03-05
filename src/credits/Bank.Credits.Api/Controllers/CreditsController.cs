using AutoMapper;
using Bank.Common.Api.Controllers;
using Bank.Common.Api.DTOs;
using Bank.Common.Auth.Attributes;
using Bank.Credits.Api.Models.Credits;
using Bank.Credits.Application.Credits;
using Bank.Credits.Application.Credits.Models;
using Microsoft.AspNetCore.Mvc;
using static Bank.Common.Application.Extensions.PagedListExtensions;

namespace Bank.Credits.Api.Controllers
{
    /// <summary>
    /// Отвечает за кредиты
    /// </summary>
    [Route("api/credits")]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    [BankAuthorize]
    public class CreditsController : BaseController
    {
        private readonly ICreditsService _creditsService;
        private readonly IMapper _mapper;

        /// <summary>
        /// Конструктор
        /// </summary>
        public CreditsController(
            ICreditsService creditsService,
            IMapper mapper)
        {
            _creditsService = creditsService;
            _mapper = mapper;
        }

        /// <summary>
        /// Получить историю кредитов
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(PagedListWithMetadata<CreditShortDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCreditsAsync([FromQuery] CreditsFilter filter, [FromQuery] Pagination pagination)
        {
            return await ExecutionResultHandlerAsync(()
                => _creditsService.GetCreditsAsync(filter, pagination.Page, pagination.Size, UserId));
        }

        /// <summary>
        /// Получить информацию по кредиту
        /// </summary>
        [HttpGet("{creditId}")]
        [ProducesResponseType(typeof(CreditDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCreditAsync([FromRoute] Guid creditId)
        {
            return await ExecutionResultHandlerAsync(()
                => _creditsService.GetCreditAsync(creditId, UserId));
        }

        /// <summary>
        /// Взять кредит
        /// </summary>
        [HttpPost("apply")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> TakeCreditAsync([FromBody] TakeCreditRequest request)
        {
            return await ExecutionResultHandlerAsync(()
                => _creditsService.TakeCreditAsync(_mapper.Map<TakeCreditDto>(request), UserId));
        }

        /// <summary>
        /// Уменьшить сумму долга по кредиту
        /// </summary>
        [HttpPost("{creditId}/reduce_payment")]
        public async Task<IActionResult> ReduceCreditAsync([FromBody] ReduceCreditRequest request, [FromRoute] Guid creditId)
        {
            return await ExecutionResultHandlerAsync(()
                => _creditsService.ReduceCreditAsync(creditId, _mapper.Map<ReduceCreditDto>(request), UserId));
        }
    }
}
