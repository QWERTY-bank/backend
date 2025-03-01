using System.Net;
using Bank.Core.Api.Infrastructure.Extensions;
using Bank.Core.Api.Infrastructure.Web;
using Bank.Core.Api.Models.Accounts;
using Bank.Core.Application.Accounts;
using Bank.Core.Application.Accounts.Models;
using Bank.Core.Application.Accounts.Read;
using Bank.Core.Application.Common;
using MediatR;
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
    private readonly IMediator _mediator;

    public AccountController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Возвращает счета пользователя
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<AccountDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorProblemDetails), (int)HttpStatusCode.Forbidden)]
    [ProducesResponseType(typeof(ErrorProblemDetails), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ErrorProblemDetails), (int)HttpStatusCode.InternalServerError)]
    public async Task<IResult> GetAccounts(CancellationToken cancellationToken)
    {
        var query = new GetMyAccountsQuery
        {
            UserId = UserId
        };
        var result = await _mediator.Send(query, cancellationToken);

        return result.ToEndpointResult();
    }
    
    /// <summary>
    /// Создает новый счет
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(typeof(long), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ErrorProblemDetails), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ErrorProblemDetails), (int)HttpStatusCode.InternalServerError)]
    public async Task<IResult> CreateAccount(
        [FromBody] CreateAccountRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateAccountCommand
        {
            Title = request.Title,
            UserId = UserId
        };
        var result = await _mediator.Send(command, cancellationToken);

        return result.ToEndpointResult();
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
    public async Task<IResult> GetAccountTransactions(
        [FromRoute] long id,
        [FromQuery] Period period,
        CancellationToken cancellationToken)
    {
        var query = new GetMyAccountTransactionsQuery
        {
            AccountId = id,
            Period = period,
            UserId = UserId
        };
        var result = await _mediator.Send(query, cancellationToken);

        return result.ToEndpointResult();
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
    [ProducesResponseType(typeof(ErrorProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ErrorProblemDetails), (int)HttpStatusCode.Forbidden)]
    [ProducesResponseType(typeof(ErrorProblemDetails), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ErrorProblemDetails), (int)HttpStatusCode.InternalServerError)]
    public async Task<IResult> DepositToAccount(
        [FromRoute] long id,
        [FromBody] DepositAccountRequest request,
        CancellationToken cancellationToken)
    {
        var command = new AddDepositTransactionCommand
        {
            AccountId = id,
            TransactionKey = request.Transaction.Key,
            Currencies = request.Transaction.CurrencyValues,
            UserId = UserId
        };
        var result = await _mediator.Send(command, cancellationToken);

        return result.ToEndpointResult();
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
    [ProducesResponseType(typeof(ErrorProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ErrorProblemDetails), (int)HttpStatusCode.Forbidden)]
    [ProducesResponseType(typeof(ErrorProblemDetails), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ErrorProblemDetails), (int)HttpStatusCode.InternalServerError)]
    public async Task<IResult> WithdrawToAccount(
        [FromRoute] long id,
        [FromBody] WithdrawAccountRequest request,
        CancellationToken cancellationToken)
    {
        var command = new AddWithdrawTransactionCommand
        {
            AccountId = id,
            TransactionKey = request.Transaction.Key,
            Currencies = request.Transaction.CurrencyValues,
            UserId = UserId
        };
        var result = await _mediator.Send(command, cancellationToken);

        return result.ToEndpointResult();
    }
    
    /// <summary>
    /// Закрывает счет
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpDelete("{id:long}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType(typeof(ErrorProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ErrorProblemDetails), (int)HttpStatusCode.Forbidden)]
    [ProducesResponseType(typeof(ErrorProblemDetails), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ErrorProblemDetails), (int)HttpStatusCode.InternalServerError)]
    public async Task<IResult> CloseAccount(
        [FromRoute] long id,
        CancellationToken cancellationToken)
    {
        var command = new CloseAccountCommand
        {
            AccountId = id,
            UserId = UserId
        };
        var result = await _mediator.Send(command, cancellationToken);
        
        return result.ToEndpointResult();
    }
}