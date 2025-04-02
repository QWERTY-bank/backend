using System.Net;
using Bank.Core.Api.Infrastructure.Auth;
using Bank.Core.Api.Infrastructure.Extensions;
using Bank.Core.Api.Infrastructure.Web;
using Bank.Core.Api.Models.Accounts;
using Bank.Core.Application.Accounts;
using Bank.Core.Application.Accounts.Models;
using Bank.Core.Application.Accounts.Unit;
using Bank.Core.Application.Accounts.Unit.Read;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Core.Api.Controllers;

/// <summary>
/// Отвечает за back2back взаимодействие cо счетами
/// </summary>
[Authorize(Policy = Policies.UnitAccount)]
[Route("api/accounts")]
public class AccountApiController : BaseController
{
    private readonly IMediator _mediator;

    public AccountApiController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Возвращает баланс счета пользователя
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("{id:long}/balance")]
    [ProducesResponseType(typeof(BalanceDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorProblemDetails), (int)HttpStatusCode.Forbidden)]
    [ProducesResponseType(typeof(ErrorProblemDetails), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ErrorProblemDetails), (int)HttpStatusCode.InternalServerError)]
    public async Task<IResult> GetAccountBalance(
        [FromRoute] long id,
        CancellationToken cancellationToken)
    {
        var query = new GetUserAccountBalanceQuery
        {
            AccountId = id
        };
        var result = await _mediator.Send(query, cancellationToken);

        return result.ToEndpointResult();
    }
    
    /// <summary>
    /// Переводит деньги на счет пользователя со счета юнита
    /// </summary>
    /// <param name="id"></param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("{id:long}/transfer/deposit")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType(typeof(ErrorProblemDetails), (int)HttpStatusCode.Forbidden)]
    [ProducesResponseType(typeof(ErrorProblemDetails), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ErrorProblemDetails), (int)HttpStatusCode.InternalServerError)]
    public async Task<IResult> UnitAccountDepositTransfer(
        [FromRoute] long id,
        [FromBody] TransactionRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UnitAccountDepositTransferCommand
        {
            Key = request.Key,
            UserAccountId = id,
            CurrencyValue = request.CurrencyValue,
            UnitId = UnitId
        };
        var result = await _mediator.Send(command, cancellationToken);
        
        return result.ToEndpointResult();
    }
    
    /// <summary>
    /// Переводит деньги на счет юнита со счета пользователя
    /// </summary>
    /// <param name="id"></param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("{id:long}/transfer/withdraw")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType(typeof(ErrorProblemDetails), (int)HttpStatusCode.Forbidden)]
    [ProducesResponseType(typeof(ErrorProblemDetails), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ErrorProblemDetails), (int)HttpStatusCode.InternalServerError)]
    public async Task<IResult> UnitAccountWithdrawTransfer(
        [FromRoute] long id,
        [FromBody] TransactionRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UnitAccountWithdrawTransferCommand
        {
            Key = request.Key,
            UserAccountId = id,
            CurrencyValue = request.CurrencyValue,
            UnitId = UnitId
        };
        var result = await _mediator.Send(command, cancellationToken);
        
        return result.ToEndpointResult();
    }
}