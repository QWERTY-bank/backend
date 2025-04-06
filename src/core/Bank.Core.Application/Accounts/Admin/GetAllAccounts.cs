using Bank.Core.Application.Accounts.Models;
using Bank.Core.Domain.Accounts;
using Bank.Core.Domain.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Bank.Core.Application.Accounts.Admin;

public class GetAllAccountsQuery : IRequest<OperationResult<IReadOnlyCollection<AdminAccountDto>>>;

public class GetAllAccountsQueryHandler : IRequestHandler<GetAllAccountsQuery, OperationResult<IReadOnlyCollection<AdminAccountDto>>>
{
    private readonly ICoreDbContext _dbContext;

    public GetAllAccountsQueryHandler(ICoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<OperationResult<IReadOnlyCollection<AdminAccountDto>>> Handle(GetAllAccountsQuery request, CancellationToken cancellationToken)
    {
        var creditAccounts = await _dbContext.UnitAccounts
            .Where(account => account.UnitId == GetCreditAccountsQueryHandler.CreditAccountId)
            .Select(account => new AdminAccountDto
            {
                Id = account.Id,
                Title = account.Title,
                UserId = null,
                IsClosed = account.IsClosed,
                CurrencyValue = account.AccountCurrencies
                    .Select(currency => new CurrencyValue
                    {
                        Code = currency.Code,
                        Value = currency.Value
                    }).Single(),
                IsMaster = true
            })
            .ToListAsync(cancellationToken);

        var userAccounts = await _dbContext.PersonalAccounts
            .OrderBy(account => account.Id)
            .Select(account => new AdminAccountDto
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
                    }).Single(),
                IsMaster = false
            })
            .ToListAsync(cancellationToken);

        var accounts = creditAccounts.Concat(userAccounts).ToArray();
        
        return OperationResultFactory.Success<IReadOnlyCollection<AdminAccountDto>>(accounts);
    }
}