using Bank.Common.Api.Controllers;
using Bank.Common.Api.DTOs;
using Bank.Credits.Api.Models.Tariffs;
using Bank.Credits.Application.Tarifs;
using Microsoft.AspNetCore.Mvc;
using static Bank.Common.Application.Extensions.PagedListExtensions;

namespace Bank.Credits.Api.Controllers
{
    /// <summary>
    /// Отвечает за тарифы для кредитов
    /// </summary>
    [Route("api/tariffs")]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public class TariffController : BaseAuthController
    {
        /// <summary>
        /// Получить список тарифов
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(PagedListWithMetadata<TariffDto>), StatusCodes.Status200OK)]
        public Task<ActionResult> GetTariffsAsync()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Создать новый тариф (для сотрудников)
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public Task<ActionResult> CreateTariffAsync([FromBody] CreateTariffRequest request)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Удалить тариф (для сотрудников)
        /// </summary>
        [HttpDelete("{tariffId}")]
        public Task<ActionResult> DeleteTariffAsync([FromRoute] Guid tariffId)
        {
            throw new NotImplementedException();
        }
    }
}
