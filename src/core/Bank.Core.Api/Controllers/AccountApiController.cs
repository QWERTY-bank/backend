using System.Net;
using Bank.Core.Api.Models.Accounts;
using Bank.Core.Application.Accounts.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Core.Api.Controllers;

/// <summary>
/// Отвечает за back2back взаимодействие cо счетами
/// </summary>
[Authorize]
[Route("api/accounts")]
public class AccountApiController : BaseController
{
    /// <summary>
    /// Возвращает баланс счет пользователя
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("{id:long}/balance")]
    [ProducesResponseType(typeof(BalanceDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public Task<IResult> GetAccounts(
        [FromRoute] long id,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Переводит деньги счета на счет
    /// </summary>
    /// <param name="id"></param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("{id:long}/transfer")]
    [ProducesResponseType(typeof(BalanceDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public Task<IResult> TransferCurrencies(
        [FromRoute] long id,
        [FromBody] TransactionRequest request,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}