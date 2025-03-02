using Bank.Core.Application.Accounts.Models;
using Bank.Core.Application.Common;
using Bank.Core.Domain.Common;
using Bank.Core.Domain.Transactions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Bank.Core.Application.Accounts;

public class UnitAccountWithdrawTransferCommand : IRequest<OperationResult<Empty>>
{
    public required Guid UnitId { get; init; }
    public required long UserAccountId { get; init; }
    public required Guid Key { get; init; }
    public required IReadOnlyCollection<CurrencyValue> Currencies { get; init; }
}

public class UnitAccountWithdrawTransferCommandHandler : IRequestHandler<UnitAccountWithdrawTransferCommand, OperationResult<Empty>>
{
    private readonly ICoreDbContext _dbContext;
    private readonly AccountService _accountService;

    public UnitAccountWithdrawTransferCommandHandler(
        ICoreDbContext dbContext, 
        AccountService accountService)
    {
        _dbContext = dbContext;
        _accountService = accountService;
    }

    public async Task<OperationResult<Empty>> Handle(
        UnitAccountWithdrawTransferCommand request, 
        CancellationToken cancellationToken)
    {
        var unitAccountId = await _dbContext.UnitAccounts
            .Where(account => account.UnitId == request.UnitId)
            .Select(account => account.Id)
            .SingleOrDefaultAsync(cancellationToken);

        if (unitAccountId == default)
        {
            return OperationResultFactory.NotFound<Empty>();
        }

        var transactionCurrencies = request.Currencies
            .Select(currency => new TransactionCurrency
            {
                Code = currency.Code,
                Value = currency.Value
            })
            .ToArray();

        var result = await _accountService.TransferCurrencies(
            fromAccountId: request.UserAccountId,
            toAccountId: unitAccountId,
            request.Key,
            transactionCurrencies,
            cancellationToken);

        return result;
    }
}