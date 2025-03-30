using System.Net;
using Bank.Core.Api.Infrastructure.Web;
using Bank.Core.Application.Abstractions;
using Bank.Core.Application.Currencies.Models;
using Bank.Core.Domain.Currencies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Core.Api.Controllers;

[Authorize]
[ApiController]
[Route("currencies")]
public class CurrencyController : BaseController
{
    private readonly ICurrencyRateService _currencyRateService;

    public CurrencyController(ICurrencyRateService currencyRateService)
    {
        _currencyRateService = currencyRateService;
    }

    /// <summary>
    /// Возвращает текущий курс валют к рублю
    /// </summary>
    [HttpGet("rate")]
    [ProducesResponseType(typeof(IReadOnlyCollection<CurrencyRateDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorProblemDetails), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ErrorProblemDetails), (int)HttpStatusCode.InternalServerError)]
    public async Task<IResult> GetCurrencyRate(CancellationToken cancellationToken)
    {
        var codes = Enum.GetValues<CurrencyCode>().ToArray();

        var currencies = await _currencyRateService.GetCurrencyRate(
            codes,
            CancellationToken.None);
        
        return TypedResults.Ok(currencies);
    }
}