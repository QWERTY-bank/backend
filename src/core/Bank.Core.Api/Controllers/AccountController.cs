using System.Net;
using Bank.Core.Api.Models.Accounts;
using Bank.Core.Api.Models.Common;
using Bank.Core.Application.Accounts.Models;
using Bank.Core.Application.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Core.Api.Controllers;

/// <summary>
/// Отвечает за счета пользователей
/// </summary>
[Authorize]
[Route("accounts")]
public class AccountController : BaseController
{
    /// <summary>
    /// Возвращает счета пользователя
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(typeof(Page<AccountDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public Task<IResult> GetAccounts(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Создает новый счет
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(typeof(long), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public Task<IResult> CreateAccount(
        [FromBody] CreateAccountRequest request,
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
    
    /// <summary>
    /// Вносит деньги на счет
    /// </summary>
    /// <param name="id"></param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("{id:long}/deposit")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public Task<IResult> DepositToAccount(
        [FromRoute] long id,
        [FromBody] DepositAccountRequest request,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Снимает деньги со счета
    /// </summary>
    /// <param name="id"></param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("{id:long}/withdraw")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public Task<IResult> WithdrawToAccount(
        [FromRoute] long id,
        [FromBody] WithdrawAccountRequest request,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}