using Bank.Core.Application.Accounts.Models;
using Bank.Core.Domain.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Bank.Core.Application.Accounts.User.Read;

public class GetMyAccountBalanceQuery : IRequest<OperationResult<MyBalanceDto>>
{
    public required Guid UserId { get; init; }
    public required long AccountId { get; init; }
}

public class GetMyAccountBalanceHandler : IRequestHandler<GetMyAccountBalanceQuery, OperationResult<MyBalanceDto>>
{
    private readonly ICoreDbContext _dbContext;

    public GetMyAccountBalanceHandler(ICoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<OperationResult<MyBalanceDto>> Handle(
        GetMyAccountBalanceQuery request,
        CancellationToken cancellationToken)
    {
        var balanceDto = await _dbContext.PersonalAccounts
            .Where(account => account.Id == request.AccountId && account.UserId == request.UserId)
            .Select(account => new MyBalanceDto()
            {
                IsClosed = account.IsClosed,
                CurrencyValue = account.AccountCurrencies
                    .Select(currency => new CurrencyValue
                    {
                        Code = currency.Code,
                        Value = currency.Value
                    })
                    .First()
            })
            .SingleOrDefaultAsync(cancellationToken);

        return balanceDto == null
            ? OperationResultFactory.NotFound<MyBalanceDto>(request.AccountId)
            : OperationResultFactory.Success(balanceDto);
    }
}