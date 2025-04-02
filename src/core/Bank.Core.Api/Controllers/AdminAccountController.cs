using System.Net;
using Bank.Common.Auth.Attributes;
using Bank.Common.Models.Auth;
using Bank.Core.Api.Infrastructure.Extensions;
using Bank.Core.Api.Infrastructure.Web;
using Bank.Core.Application.Accounts.Admin;
using Bank.Core.Application.Accounts.Models;
using Bank.Core.Application.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Core.Api.Controllers;

[BankAuthorize(RoleType.Employee)]
[Route("admin/accounts")]
public class AdminAccountController : BaseController
{
    private readonly IMediator _mediator;

    public AdminAccountController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    /// <summary>
    /// Возвращает все счета
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<AccountDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorProblemDetails), (int)HttpStatusCode.Forbidden)]
    [ProducesResponseType(typeof(ErrorProblemDetails), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ErrorProblemDetails), (int)HttpStatusCode.InternalServerError)]
    public async Task<IResult> GetAllAccounts(CancellationToken cancellationToken)
    {
        var query = new GetAllAccountsQuery();
        var result = await _mediator.Send(query, cancellationToken);

        return result.ToEndpointResult();
    }
    
    /// <summary>
    /// Возвращает мастер счета банка
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("master")]
    [ProducesResponseType(typeof(IReadOnlyCollection<UnitAccountDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorProblemDetails), (int)HttpStatusCode.Forbidden)]
    [ProducesResponseType(typeof(ErrorProblemDetails), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ErrorProblemDetails), (int)HttpStatusCode.InternalServerError)]
    public async Task<IResult> GetUnitAccounts(CancellationToken cancellationToken)
    {
        var query = new GetCreditAccountsQuery();
        var result = await _mediator.Send(query, cancellationToken);

        return result.ToEndpointResult();
    }

    /// <summary>
    /// Возвращает счета пользователя
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("users/{id}")]
    [ProducesResponseType(typeof(IReadOnlyCollection<AccountDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorProblemDetails), (int)HttpStatusCode.Forbidden)]
    [ProducesResponseType(typeof(ErrorProblemDetails), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ErrorProblemDetails), (int)HttpStatusCode.InternalServerError)]
    public async Task<IResult> GetAccounts([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var query = new GetUserAccountsQuery
        {
            UserId = id
        };
        var result = await _mediator.Send(query, cancellationToken);

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
        var query = new GetUserAccountTransactionsQuery
        {
            AccountId = id,
            Period = period,
        };
        var result = await _mediator.Send(query, cancellationToken);

        return result.ToEndpointResult();
    }
}