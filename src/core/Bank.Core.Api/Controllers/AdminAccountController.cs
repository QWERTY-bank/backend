using System.Net;
using Bank.Common.Auth.Attributes;
using Bank.Common.Models.Auth;
using Bank.Core.Api.Infrastructure.Extensions;
using Bank.Core.Api.Infrastructure.Web;
using Bank.Core.Application.Accounts.Admin;
using Bank.Core.Application.Accounts.Models;
using Bank.Core.Application.Common;
using Bank.Core.Domain.Currencies;
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
    /// Возвращает все счета (пока не реализовано, возвращает моковые данные)
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<AccountDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorProblemDetails), (int)HttpStatusCode.Forbidden)]
    [ProducesResponseType(typeof(ErrorProblemDetails), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ErrorProblemDetails), (int)HttpStatusCode.InternalServerError)]
    public async Task<IReadOnlyCollection<AccountDto>> GetAllAccounts(CancellationToken cancellationToken)
    {
        return
        [
            new AccountDto
            {
                Id = 1,
                CurrencyValue = new CurrencyValue
                {
                    Value = 1000,
                    Code = CurrencyCode.Rub
                },
                IsClosed = false,
                Title = "Test Account",
                UserId = Guid.NewGuid()
            }
        ];
    }
    
    /// <summary>
    /// Возвращает мастер счета банка (пока не реализовано, возвращает моковые данные)
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("master")]
    [ProducesResponseType(typeof(IReadOnlyCollection<UnitAccountDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorProblemDetails), (int)HttpStatusCode.Forbidden)]
    [ProducesResponseType(typeof(ErrorProblemDetails), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ErrorProblemDetails), (int)HttpStatusCode.InternalServerError)]
    public async Task<IReadOnlyCollection<UnitAccountDto>> GetUnitAccounts(CancellationToken cancellationToken)
    {
        var unitId = Guid.NewGuid();
        
        return
        [
            new UnitAccountDto
            {
                Id = 1,
                CurrencyValue = new CurrencyValue
                {
                    Value = 1000,
                    Code = CurrencyCode.Rub
                },
                UnitId = unitId,
                Title = "MasterRub"
            },
            new UnitAccountDto
            {
                Id = 1,
                CurrencyValue = new CurrencyValue
                {
                    Value = 1000,
                    Code = CurrencyCode.Eur
                },
                UnitId = unitId,
                Title = "MasterEur"
            },
            new UnitAccountDto
            {
                Id = 1,
                CurrencyValue = new CurrencyValue
                {
                    Value = 1000,
                    Code = CurrencyCode.Usd
                },
                UnitId = unitId,
                Title = "MasterUsd"
            }
        ];
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