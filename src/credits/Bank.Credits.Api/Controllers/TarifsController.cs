using Bank.Common.Api.Controllers;
using Bank.Common.Api.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        public Task<ActionResult> GetTarifsAsync()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Получить подробную информацию о тарифе
        /// </summary>
        [HttpGet("{tarifId}")]
        public Task<ActionResult> GetTarifAsync([FromRoute] Guid tarifId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Создать новый тариф (для сотрудников)
        /// </summary>
        [HttpPost]
        public Task<ActionResult> CreateTarifAsync()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Обновить информацию о тарифе (для сотрудников)
        /// </summary>
        [HttpPut("{tarifId}")]
        public Task<ActionResult> UpdateTarifAsync([FromRoute] Guid tarifId)
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
