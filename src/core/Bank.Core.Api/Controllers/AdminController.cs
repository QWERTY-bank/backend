using System.Net;
using Bank.Common.Api.DTOs;
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
    [ProducesResponseType(typeof(Page<AccountDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public Task<IResult> GetAccounts(
        [FromRoute] Guid id,
        [FromQuery] Pagination pagination,
        CancellationToken cancellationToken)
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
    [ProducesResponseType(typeof(Page<TransactionDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public Task<IResult> GetAccountTransactions(
        [FromRoute] long id,
        [FromQuery] Period period,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}