using Bank.Core.Application.Accounts.Models;
using Bank.Core.Domain.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Bank.Core.Application.Accounts.Admin;

public class GetCreditAccountsQuery : IRequest<OperationResult<IReadOnlyCollection<UnitAccountDto>>>;

public class GetCreditAccountsQueryHandler : IRequestHandler<GetCreditAccountsQuery, OperationResult<IReadOnlyCollection<UnitAccountDto>>>
{
    public static Guid CreditAccountId { get; } = Guid.Parse("a99fb2a5-c52e-4168-8ac9-b28878d3b407");
    
    private readonly ICoreDbContext _dbContext;

    public GetCreditAccountsQueryHandler(ICoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<OperationResult<IReadOnlyCollection<UnitAccountDto>>> Handle(GetCreditAccountsQuery request, CancellationToken cancellationToken)
    {
        var accounts = await _dbContext.UnitAccounts
            .Where(account => account.UnitId == CreditAccountId)
            .Select(account => new UnitAccountDto
            {
                Id = account.Id,
                Title = account.Title,
                UnitId = account.UnitId,
                CurrencyValue = account.AccountCurrencies
                    .Select(currency => new CurrencyValue
                    {
                        Code = currency.Code,
                        Value = currency.Value
                    }).Single()
            })
            .ToListAsync(cancellationToken);

        return OperationResultFactory.Success<IReadOnlyCollection<UnitAccountDto>>(accounts);
    }
}