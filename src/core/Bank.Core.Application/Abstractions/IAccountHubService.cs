using Bank.Core.Domain.Accounts;
using Bank.Core.Domain.Transactions;

namespace Bank.Core.Application.Abstractions;

public interface IAccountHubService
{
    public Task NotifyUsers(
        long accountId,
        TransactionEntity transaction,
        AccountCurrencyEntity accountCurrency);
}