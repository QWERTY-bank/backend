using Bank.Core.Application.Accounts.Models;
using Bank.Core.Domain.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Bank.Core.Application.Accounts.Admin;

public class GetAllAccountsQuery : IRequest<OperationResult<IReadOnlyCollection<AccountDto>>>;

public class GetAllAccountsQueryHandler : IRequestHandler<GetAllAccountsQuery, OperationResult<IReadOnlyCollection<AccountDto>>>
{
    private readonly ICoreDbContext _dbContext;

    public GetAllAccountsQueryHandler(ICoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<OperationResult<IReadOnlyCollection<AccountDto>>> Handle(GetAllAccountsQuery request, CancellationToken cancellationToken)
    {
        var accounts = await _dbContext.PersonalAccounts
            .Select(account => new AccountDto
            {
                Id = account.Id,
                Title = account.Title,
                UserId = account.UserId,
                IsClosed = account.IsClosed,
                CurrencyValue = account.AccountCurrencies
                    .Select(currency => new CurrencyValue
                    {
                        Code = currency.Code,
                        Value = currency.Value
                    }).Single()
            })
            .ToListAsync(cancellationToken);
        
        return OperationResultFactory.Success<IReadOnlyCollection<AccountDto>>(accounts);
    }
}