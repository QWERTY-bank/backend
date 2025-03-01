using Bank.Core.Domain.Common;
using Bank.Core.Domain.Transactions;

namespace Bank.Core.Domain.Accounts;

public abstract class AccountBaseEntity : BaseEntity<long>
{
    public required string Title { get; init; }
    public bool IsClosed { get; set; } = false;
    public List<TransactionEntity> Transactions { get; init; } = [];
    public List<AccountCurrencyEntity> AccountCurrencies { get; init; } = [];
}