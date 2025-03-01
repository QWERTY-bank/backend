using Bank.Core.Application.Accounts.Models;
using Bank.Core.Domain.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Bank.Core.Application.Accounts.Read;

public class GetUserAccountsQuery : IRequest<OperationResult<IReadOnlyCollection<AccountDto>>>
{
    public Guid UserId { get; init; }
}

public class GetUserAccountsQueryHandler : IRequestHandler<GetUserAccountsQuery, OperationResult<IReadOnlyCollection<AccountDto>>>
{
    private readonly ICoreDbContext _dbContext;

    public GetUserAccountsQueryHandler(ICoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<OperationResult<IReadOnlyCollection<AccountDto>>> Handle(GetUserAccountsQuery request, CancellationToken cancellationToken)
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
                CurrencyValues = account.AccountCurrencies.Select(currency => new CurrencyValueDto
                    {
                        Code = currency.Code,
                        Value = currency.Value
                    })
                    .ToArray()
            })
            .ToListAsync(cancellationToken);

        return OperationResultFactory.Success<IReadOnlyCollection<AccountDto>>(personalAccounts);
    }
}