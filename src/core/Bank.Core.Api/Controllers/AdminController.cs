using System.Net;
using Bank.Core.Api.Infrastructure.Web;
using Bank.Core.Application.Accounts.Models;
using Bank.Core.Application.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Core.Api.Controllers;

[Authorize]
[Route("admin/accounts")]
public class AdminController : BaseController
{
    /// <summary>
    /// Возвращает счета пользователя
    /// </summary>
    /// <param name="id"></param>
    /// <param name="pagination"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("users/{id}")]
    [ProducesResponseType(typeof(IReadOnlyCollection<AccountDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorProblemDetails), (int)HttpStatusCode.Forbidden)]
    [ProducesResponseType(typeof(ErrorProblemDetails), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ErrorProblemDetails), (int)HttpStatusCode.InternalServerError)]
    public Task<IResult> GetAccounts([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Возвращает транзакции счета
    /// </summary>
    /// <param name="id"></param>
    /// <param name="period"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("{id:long}/transactions")]
    [ProducesResponseType(typeof(TransactionsResponseDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorProblemDetails), (int)HttpStatusCode.Forbidden)]
    [ProducesResponseType(typeof(ErrorProblemDetails), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ErrorProblemDetails), (int)HttpStatusCode.InternalServerError)]
    public Task<IResult> GetAccountTransactions(
        [FromRoute] long id,
        [FromQuery] Period period,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}