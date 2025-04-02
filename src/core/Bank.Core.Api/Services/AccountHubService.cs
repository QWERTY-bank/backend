using Bank.Core.Api.Hubs;
using Bank.Core.Api.Hubs.Models;
using Bank.Core.Application.Abstractions;
using Bank.Core.Application.Accounts.Models;
using Bank.Core.Domain.Accounts;
using Bank.Core.Domain.Transactions;
using Microsoft.AspNetCore.SignalR;

namespace Bank.Core.Api.Services;

public class AccountHubService : IAccountHubService
{
    private readonly IHubContext<AccountHub, IAccountHub> _accountHubContext;

    public AccountHubService(IHubContext<AccountHub, IAccountHub> accountHubContext)
    {
        _accountHubContext = accountHubContext;
    }

    public Task NotifyUsers(
        long accountId, 
        TransactionEntity transaction, 
        AccountCurrencyEntity accountCurrency)
    {
        var currency = transaction.Currencies.Single();

        var transactionResponse = new TransactionHubResponse
        {
            Transaction = new TransactionDto
            {
                CurrencyValue = new CurrencyValue
                {
                    Code = currency.Code,
                    Value = currency.Value
                },
                Key = transaction.Key,
                Type = transaction.Type
            },
            CurrencyValue = new CurrencyValue
            {
                Code = accountCurrency.Code,
                Value = accountCurrency.Value
            }
        };

        return _accountHubContext.Clients.Group(accountId.ToString()).ReceiveTransaction(transactionResponse);
    }
}