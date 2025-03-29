using Bank.Core.Application.Accounts.Models;
using Bank.Core.Domain.Common;
using Bank.Core.Domain.Transactions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Bank.Core.Application.Accounts.User;

public class AddWithdrawTransactionCommand : IRequest<OperationResult<Empty>>
{
    public required long AccountId { get; init; }
    public required Guid UserId { get; init; }
    public required Guid TransactionKey { get; init; }
    public required IReadOnlyCollection<CurrencyValue> Currencies { get; init; }
}

public class AddWithdrawTransactionCommandHandler : IRequestHandler<AddWithdrawTransactionCommand, OperationResult<Empty>>
{
    private readonly ICoreDbContext _dbContext;

    public AddWithdrawTransactionCommandHandler(ICoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<OperationResult<Empty>> Handle(
        AddWithdrawTransactionCommand request,
        CancellationToken cancellationToken)
    {
        var accountExists = await _dbContext.PersonalAccounts
            .AnyAsync(
                account => account.Id == request.AccountId && !account.IsClosed && account.UserId == request.UserId,
                cancellationToken);

        if (!accountExists)
        {
            return OperationResultFactory.NotFound<Empty>(request.AccountId);
        }

        await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
        
        await _dbContext.SelectAccountForUpdate(
            request.AccountId,
            cancellationToken);
        
        var account = await _dbContext.PersonalAccounts
            .Include(account => account.AccountCurrencies)
            .SingleAsync(
                account => account.Id == request.AccountId,
                cancellationToken);

        var withdrawTransaction = new WithdrawTransactionEntity
        {
            AccountId = request.AccountId,
            Key = request.TransactionKey,
            CreatedDate = DateTime.UtcNow,
            OperationDate = DateTime.UtcNow,
            Currencies = request.Currencies
                .Select(currency => new TransactionCurrency
                {
                    Code = currency.Code,
                    Value = currency.Value
                })
                .ToList()
        };

        var withdrawResult = account.Withdraw(withdrawTransaction);
        if (withdrawResult.IsError)
        {
            return withdrawResult;
        }
        
        try
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            
            return OperationResultFactory.EmptyResult;
        }
        catch (DbUpdateException e)
        {
            var transactionExists = e.InnerException is PostgresException
            {
                ConstraintName: "ak_transaction_entity_key"
            };

            if (!transactionExists)
            {
                throw;
            }

            var existingTransaction = await _dbContext.Transactions
                .SingleAsync(
                    existingTransaction => existingTransaction.Key == request.TransactionKey,
                    cancellationToken);

            if (existingTransaction.Type != TransactionType.Withdraw ||
                existingTransaction.AccountId != request.AccountId ||
                existingTransaction.Currencies.Except(withdrawTransaction.Currencies).Any())
            {
                return OperationResultFactory.InvalidData<Empty>(
                    $"Транзакция с ключом {request.TransactionKey} не добавлена, т.к. транзакция с таким же ключом, но другим составом была добавлена ранее.");
            }
                
            return OperationResultFactory.EmptyResult;
        }
    }
}