using Bank.Core.Domain.Common;
using Bank.Core.Domain.Transactions;

namespace Bank.Core.Domain.Accounts;

public class AccountEntity : BaseEntity<long>
{
    public required Guid UserId { get; init; }
    public required string Title { get; init; }
    public List<TransactionEntity> Transactions { get; init; } = [];
    public List<AccountCurrencyEntity> AccountCurrencies { get; init; } = [];
}