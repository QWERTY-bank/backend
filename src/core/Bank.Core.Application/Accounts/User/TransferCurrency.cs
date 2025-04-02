using Bank.Core.Application.Accounts.Models;
using Bank.Core.Application.Common;
using Bank.Core.Domain.Common;
using Bank.Core.Domain.Transactions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Bank.Core.Application.Accounts.User;

public class TransferCurrencyCommand : IRequest<OperationResult<Empty>>
{
    public required Guid UserId { get; init; }
    public required long FromAccountId { get; init; }
    
    public required long ToAccountId { get; init; }

    public required Guid TransactionKey { get; init; }
    public required CurrencyValue CurrencyValue { get; init; }
}

public class TransferCurrencyCommandHandler : IRequestHandler<TransferCurrencyCommand, OperationResult<Empty>>
{
    private readonly ICoreDbContext _dbContext;
    private readonly AccountService _accountService;

    public TransferCurrencyCommandHandler(
        ICoreDbContext dbContext, 
        AccountService accountService)
    {
        _dbContext = dbContext;
        _accountService = accountService;
    }

    public async Task<OperationResult<Empty>> Handle(
        TransferCurrencyCommand request,
        CancellationToken cancellationToken)
    {
        var userAccountExists = await _dbContext.PersonalAccounts
            .AnyAsync(account => account.Id == request.FromAccountId &&
                                 account.UserId == request.UserId &&
                                 !account.IsClosed &&
                                 account.AccountCurrencies.Any(currency => currency.Code == request.CurrencyValue.Code),
                cancellationToken);

        if (!userAccountExists)
        {
            return OperationResultFactory.NotFound<Empty>();
        }
        
        var transactionCurrency = new TransactionCurrency
        {
            Code = request.CurrencyValue.Code,
            Value = request.CurrencyValue.Value
        };

        var result = await _accountService.TransferCurrencies(
            fromAccountId: request.FromAccountId,
            toAccountId: request.ToAccountId,
            request.TransactionKey,
            transactionCurrency,
            cancellationToken);

        return result;
    }
}