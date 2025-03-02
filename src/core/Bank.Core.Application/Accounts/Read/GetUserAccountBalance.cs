using Bank.Core.Application.Accounts.Models;
using Bank.Core.Domain.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Bank.Core.Application.Accounts.Read;

public class GetUserAccountBalanceQuery : IRequest<OperationResult<BalanceDto>>
{
    public long AccountId { get; init; }
}

public class GetUserAccountBalanceQueryHandler : IRequestHandler<GetUserAccountBalanceQuery, OperationResult<BalanceDto>>
{
    private readonly ICoreDbContext _dbContext;

    public GetUserAccountBalanceQueryHandler(ICoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<OperationResult<BalanceDto>> Handle(
        GetUserAccountBalanceQuery request,
        CancellationToken cancellationToken)
    {
        var balanceDto = await _dbContext.PersonalAccounts
            .Where(account => account.Id == request.AccountId)
            .Select(account => new BalanceDto
            {
                UserId = account.UserId,
                IsClosed = account.IsClosed,
                CurrencyValues = account.AccountCurrencies
                    .Select(currency => new CurrencyValue
                    {
                        Code = currency.Code,
                        Value = currency.Value
                    })
                    .ToArray()
            })
            .SingleOrDefaultAsync(cancellationToken);

        return balanceDto == null
            ? OperationResultFactory.NotFound<BalanceDto>(request.AccountId)
            : OperationResultFactory.Success(balanceDto);
    }
}