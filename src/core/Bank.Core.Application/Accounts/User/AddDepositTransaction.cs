using Bank.Core.Application.Abstractions;
using Bank.Core.Application.Accounts.Models;
using Bank.Core.Domain.Common;
using Bank.Core.Domain.Transactions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Bank.Core.Application.Accounts.User;

public class AddDepositTransactionCommand : IRequest<OperationResult<Empty>>
{
    public required long AccountId { get; init; }
    public required Guid UserId { get; init; }
    public required Guid TransactionKey { get; init; }
    public required IReadOnlyCollection<CurrencyValue> Currencies { get; init; }
}

public class AddDepositTransactionCommandHandler : IRequestHandler<AddDepositTransactionCommand, OperationResult<Empty>>
{
    private readonly ICoreDbContext _dbContext;
    private readonly IAccountHubService _accountHubService;

    public AddDepositTransactionCommandHandler(ICoreDbContext dbContext, IAccountHubService accountHubService)
    {
        _dbContext = dbContext;
        _accountHubService = accountHubService;
    }

    public async Task<OperationResult<Empty>> Handle(
        AddDepositTransactionCommand request,
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

        var depositTransaction = new DepositTransactionEntity
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

        var depositResult = account.Deposit(depositTransaction);
        if (depositResult.IsError)
        {
            return depositResult;
        }

        try
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            await _accountHubService.NotifyUsers(
                request.AccountId,
                depositTransaction, 
                account.AccountCurrencies.Single());
            
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

            if (existingTransaction.Type != TransactionType.Deposit ||
                existingTransaction.AccountId != request.AccountId ||
                existingTransaction.Currencies.Except(depositTransaction.Currencies).Any())
            {
                return OperationResultFactory.InvalidData<Empty>(
                    $"Транзакция с ключом {request.TransactionKey} не добавлена, т.к. транзакция с таким же ключом, но другим составом была добавлена ранее.");
            }
                
            return OperationResultFactory.EmptyResult;
        }
    }
}