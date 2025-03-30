using Bank.Core.Application.Accounts.Models;
using Bank.Core.Application.Common;
using Bank.Core.Domain.Common;
using Bank.Core.Domain.Transactions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Bank.Core.Application.Accounts.Unit;

public class UnitAccountDepositTransferCommand : IRequest<OperationResult<Empty>>
{
    public required Guid UnitId { get; init; }
    public required long UserAccountId { get; init; }
    public required Guid Key { get; init; }
    public required CurrencyValue CurrencyValue { get; init; }
}

public class UnitAccountDepositTransferCommandHandler : IRequestHandler<UnitAccountDepositTransferCommand, OperationResult<Empty>>
{
    private readonly ICoreDbContext _dbContext;
    private readonly AccountService _accountService;

    public UnitAccountDepositTransferCommandHandler(
        ICoreDbContext dbContext, 
        AccountService accountService)
    {
        _dbContext = dbContext;
        _accountService = accountService;
    }

    public async Task<OperationResult<Empty>> Handle(
        UnitAccountDepositTransferCommand request, 
        CancellationToken cancellationToken)
    {
        var unitAccountId = await _dbContext.UnitAccounts
            .Where(account => account.UnitId == request.UnitId && account.AccountCurrencies.Any(currency => currency.Code == request.CurrencyValue.Code))
            .Select(account => account.Id)
            .SingleOrDefaultAsync(cancellationToken);

        if (unitAccountId == default)
        {
            return OperationResultFactory.NotFound<Empty>();
        }

        var transactionCurrency = new TransactionCurrency
        {
            Code = request.CurrencyValue.Code,
            Value = request.CurrencyValue.Value
        };

        var result = await _accountService.TransferCurrencies(
            fromAccountId: unitAccountId,
            toAccountId: request.UserAccountId,
            request.Key,
            transactionCurrency,
            cancellationToken);

        return result;
    }
}