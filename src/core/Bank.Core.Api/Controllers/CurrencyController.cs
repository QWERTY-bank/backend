using System.Net;
using Bank.Core.Application.Currencies.Models;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Core.Api.Controllers;


/// <summary>
/// Отвечает за доступные валюты
/// </summary>
[Route("currencies")]
public class CurrencyController : BaseController
{
    /// <summary>
    /// Возвращает доступные валюты
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<CurrencyDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public Task<IResult> GetCurrencies(
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}