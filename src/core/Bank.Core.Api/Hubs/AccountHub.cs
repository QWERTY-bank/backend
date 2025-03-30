using Bank.Common.Auth.Extensions;
using Bank.Common.Models.Auth;
using Bank.Core.Api.Hubs.Models;
using Bank.Core.Application;
using Bank.Core.Application.Accounts.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Bank.Core.Api.Hubs;

public interface IAccountHub
{
    public Task ReceiveTransaction(TransactionHubResponse transaction);
    public Task ReceiveTransactions(TransactionsHubResponse transactions);
}

[Authorize]
public class AccountHub : Hub<IAccountHub>
{
    private readonly ICoreDbContext _dbContext;

    public AccountHub(ICoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SubscribeToAccount(SubscribeToAccountModel subscribeToAccountModel)
    {
        if (Context.User == null)
        {
            return;
        }
        
        var role = Context.User.GetUserMaxRole();

        var hasEmployeeRole = role >= RoleType.Employee;

        if (!hasEmployeeRole)
        {
            var userId = Context.User.GetUserId();
            
            var userAccountExists = await _dbContext.PersonalAccounts
                .AnyAsync(account => account.Id == subscribeToAccountModel.AccountId && account.UserId == userId);

            if (!userAccountExists)
            {
                return;
            }
        }

        var account = await _dbContext.Accounts
            .AsNoTracking()
            .Include(account => account.AccountCurrencies)
            .Include(account => account.Transactions)
            .Where(account => account.Id == subscribeToAccountModel.AccountId)
            .SingleOrDefaultAsync();

        if (account == null)
        {
            return;
        }
        
        var transactionHubResponse = new TransactionsHubResponse
        {
            CurrencyValue = account.AccountCurrencies
                .Select(currency => new CurrencyValue
                {
                    Value = currency.Value,
                    Code = currency.Code
                })
                .Single(),
            Transactions = account.Transactions
                .Select(transaction => new TransactionDto
                {
                    Key = transaction.Key,
                    CurrencyValue = transaction.Currencies
                        .Select(currency => new CurrencyValue
                        {
                            Value = currency.Value,
                            Code = currency.Code
                        })
                        .Single(),
                    Type = transaction.Type
                })
                .ToArray()
        };

        await Clients.Client(Context.ConnectionId).ReceiveTransactions(transactionHubResponse);
        
        await Groups.AddToGroupAsync(Context.ConnectionId, subscribeToAccountModel.AccountId.ToString());
    }
}