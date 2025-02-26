using Bank.Common.Api.Controllers;
using Bank.Common.Api.DTOs;
using Bank.Credits.Api.Models.Tarifs;
using Bank.Credits.Application.Tarifs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Bank.Common.Application.Extensions.PagedListExtensions;

namespace Bank.Credits.Api.Controllers
{
    [Route("api/tarifs")]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    [Authorize]
    public class TarifsController : BaseController
    {
        /// <summary>
        /// Получить список тарифов
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(PagedListWithMetadata<TarifDto>), StatusCodes.Status200OK)]
        public Task<ActionResult> GetTarifsAsync()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Создать новый тариф (для сотрудников)
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public Task<ActionResult> CreateTarifAsync([FromBody] CreateTarifRequest request)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Удалить тариф (для сотрудников)
        /// </summary>
        [HttpDelete("{tarifId}")]
        public Task<ActionResult> DeleteTarifAsync([FromRoute] Guid tarifId)
        {
            throw new NotImplementedException();
        }
    }
}
