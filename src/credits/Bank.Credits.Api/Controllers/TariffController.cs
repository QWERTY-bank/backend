using AutoMapper;
using Bank.Common.Api.Controllers;
using Bank.Common.Api.DTOs;
using Bank.Common.Auth.Attributes;
using Bank.Credits.Api.Models.Tariffs;
using Bank.Credits.Application.Tariffs;
using Bank.Credits.Application.Tariffs.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Bank.Common.Application.Extensions.PagedListExtensions;

namespace Bank.Credits.Api.Controllers
{
    /// <summary>
    /// Отвечает за тарифы для кредитов
    /// </summary>
    [Route("api/tariffs")]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    [BankAuthorize]
    [AllowAnonymous]
    public class TariffController : BaseController
    {
        private readonly ITariffsService _tariffsService;
        private readonly IMapper _mapper;

        /// <summary>
        /// Конструктор
        /// </summary>
        public TariffController(
            ITariffsService tariffsService,
            IMapper mapper)
        {
            _tariffsService = tariffsService;
            _mapper = mapper;
        }

        /// <summary>
        /// Получить список тарифов
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(PagedListWithMetadata<TariffDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTariffsAsync([FromQuery] TariffsFilters filters, [FromQuery] Pagination pagination)
        {
            return await ExecutionResultHandlerAsync(()
                => _tariffsService.GetTariffsAsync(filters, pagination.Page, pagination.Size));
        }

        /// <summary>
        /// Создать новый тариф (для сотрудников)
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> CreateTariffAsync([FromBody] CreateTariffRequest request)
        {
            return await ExecutionResultHandlerAsync(()
                => _tariffsService.CreateTariffAsync(_mapper.Map<CreateTariffDto>(request)));
        }

        /// <summary>
        /// Удалить тариф (для сотрудников)
        /// </summary>
        [HttpDelete("{tariffId}")]
        public async Task<IActionResult> DeleteTariffAsync([FromRoute] Guid tariffId)
        {
            return await ExecutionResultHandlerAsync(()
                => _tariffsService.DeleteTariffAsync(tariffId));
        }
    }
}
