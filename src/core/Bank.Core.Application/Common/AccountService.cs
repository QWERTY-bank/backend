using Bank.Core.Domain.Common;
using Bank.Core.Domain.Transactions;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Bank.Core.Application.Common;

public class AccountService
{
    private readonly ICoreDbContext _dbContext;

    public AccountService(ICoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<OperationResult<Empty>> TransferCurrencies(
        long fromAccountId,
        long toAccountId,
        Guid transactionKey,
        IReadOnlyCollection<TransactionCurrency> currencies,
        CancellationToken cancellationToken)
    {
        if (fromAccountId == toAccountId)
        {
            return OperationResultFactory.InvalidData<Empty>("Нельзя перевести валюту на тот же самый счет");
        }
        
        long[] accountIds = [fromAccountId, toAccountId];
        
        await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

        await _dbContext.SelectAccountsForUpdate(
            accountIds,
            cancellationToken);

        var accountById = await _dbContext.Accounts
            .Where(account => accountIds.Contains(account.Id) && !account.IsClosed)
            .Include(account => account.AccountCurrencies)
            .ToDictionaryAsync(
                account => account.Id,
                cancellationToken);

        if (!accountById.TryGetValue(fromAccountId, out var fromAccount) || !accountById.TryGetValue(toAccountId, out var toAccount))
        {
            await transaction.RollbackAsync(cancellationToken);
            
            return OperationResultFactory.NotFound<Empty>();
        }

        var withdrawTransaction = new WithdrawTransactionEntity
        {
            AccountId = fromAccountId,
            Key = transactionKey,
            CreatedDate = DateTime.UtcNow,
            OperationDate = DateTime.UtcNow,
            Currencies = currencies
        };
        var withdrawResult = fromAccount.Withdraw(withdrawTransaction);
        if (withdrawResult.IsError)
        {
            return withdrawResult;
        }
        
        var depositTransaction = new DepositTransactionEntity
        {
            ParentKey = transactionKey,
            AccountId = toAccountId,
            Key = Guid.NewGuid(),
            CreatedDate = DateTime.UtcNow,
            OperationDate = DateTime.UtcNow,
            Currencies = currencies
        };
        var depositResult = toAccount.Deposit(depositTransaction);
        if (depositResult.IsError)
        {
            return depositResult;
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
                    existingTransaction => existingTransaction.Key == transactionKey,
                    cancellationToken);

            if (existingTransaction.Type != TransactionType.Withdraw ||
                existingTransaction.AccountId != fromAccountId ||
                existingTransaction.Currencies.Except(depositTransaction.Currencies).Any())
            {
                return OperationResultFactory.InvalidData<Empty>(
                    $"Транзакция с ключом {transactionKey} не добавлена, т.к. транзакция с таким же ключом, но другим составом была добавлена ранее.");
            }
                
            return OperationResultFactory.EmptyResult;
        }
    }
}