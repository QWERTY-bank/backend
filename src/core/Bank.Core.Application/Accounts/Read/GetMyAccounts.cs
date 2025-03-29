using Bank.Core.Application.Accounts.Models;
using Bank.Core.Domain.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Bank.Core.Application.Accounts.Read;

public class GetMyAccountsQuery : IRequest<OperationResult<IReadOnlyCollection<AccountDto>>>
{
    public required Guid UserId { get; init; }
}

public class GetMyAccountsQueryHandler : IRequestHandler<GetMyAccountsQuery, OperationResult<IReadOnlyCollection<AccountDto>>>
{
    private readonly ICoreDbContext _dbContext;

    public GetMyAccountsQueryHandler(ICoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<OperationResult<IReadOnlyCollection<AccountDto>>> Handle(GetMyAccountsQuery request, CancellationToken cancellationToken)
    {
        var personalAccounts = await _dbContext.PersonalAccounts
            .AsNoTracking()
            .Where(account => account.UserId == request.UserId)
            .Select(account => new AccountDto
            {
                Id = account.Id,
                IsClosed = account.IsClosed,
                Title = account.Title,
                UserId = account.UserId,
                CurrencyValue = account.AccountCurrencies.Select(currency => new CurrencyValue
                    {
                        Code = currency.Code,
                        Value = currency.Value
                    })
                    .First()
            })
            .ToListAsync(cancellationToken);

        return OperationResultFactory.Success<IReadOnlyCollection<AccountDto>>(personalAccounts);
    }
}