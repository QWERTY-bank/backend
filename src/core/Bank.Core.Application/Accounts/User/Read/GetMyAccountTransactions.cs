using Bank.Core.Application.Accounts.Models;
using Bank.Core.Application.Common;
using Bank.Core.Domain.Common;
using Bank.Core.Domain.Transactions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Bank.Core.Application.Accounts.User.Read;

public class GetMyAccountTransactionsQuery : IRequest<OperationResult<TransactionsResponseDto>>
{
    public required Guid UserId { get; init; }
    public required long AccountId { get; init; }
    public required Period Period { get; init; }
}

public class GetMyAccountTransactionsQueryHandler : IRequestHandler<GetMyAccountTransactionsQuery, OperationResult<TransactionsResponseDto>>
{
    private readonly ICoreDbContext _dbContext;

    public GetMyAccountTransactionsQueryHandler(ICoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<OperationResult<TransactionsResponseDto>> Handle(
        GetMyAccountTransactionsQuery request, 
        CancellationToken cancellationToken)
    {
        var accountExists = await _dbContext.PersonalAccounts
            .AnyAsync(
                account => account.Id == request.AccountId && account.UserId == request.UserId,
                cancellationToken);

        if (!accountExists)
        {
            return OperationResultFactory.NotFound<TransactionsResponseDto>(request.AccountId);
        }
        
        var transactions = await _dbContext.Transactions
            .AsNoTracking()
            .Where(transaction => transaction.AccountId == request.AccountId)
            .Where(transaction =>
                transaction.OperationDate >= request.Period.Start &&
                transaction.OperationDate <= request.Period.End)
            .OrderByDescending(transaction => transaction.OperationDate)
            .ToListAsync(cancellationToken);

        var groupedTransactions = transactions
            .SelectMany(transaction => transaction.Currencies.Select(currency => new
            {
                TransactionType = transaction.Type,
                Currency = currency
            }))
            .GroupBy(resource => resource.Currency.Code)
            .ToArray();

        var depositTotals = groupedTransactions
            .Select(grouping =>
                new CurrencyValue
                {
                    Code = grouping.Key,
                    Value = grouping
                        .Where(currency => currency.TransactionType == TransactionType.Deposit)
                        .Sum(currency => currency.Currency.Value)
                })
            .ToArray();

        var withdrawTotals = groupedTransactions
            .Select(grouping =>
                new CurrencyValue
                {
                    Code = grouping.Key,
                    Value = grouping
                        .Where(currency => currency.TransactionType == TransactionType.Withdraw)
                        .Sum(currency => currency.Currency.Value)
                })
            .ToArray();

        var transactionsResponse = new TransactionsResponseDto
        {
            Transactions = transactions
                .Select(transaction => new TransactionDto
                {
                    Key = transaction.Key,
                    Type = transaction.Type,
                    CurrencyValue = transaction.Currencies
                        .Select(currency => new CurrencyValue
                        {
                            Code = currency.Code,
                            Value = currency.Value
                        })
                        .First()
                })
                .ToArray(),
            DepositCurrencyTotals = depositTotals.First(),
            WithdrawCurrencyTotals = withdrawTotals.First()
        };

        return OperationResultFactory.Success(transactionsResponse);
    }
}



