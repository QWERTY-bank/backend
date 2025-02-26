using Bank.Core.Domain.Common;

namespace Bank.Core.Domain.Accounts;

public class AccountCurrencyEntity : BaseEntity<long>
{
    public required decimal Value { get; set; }
    public required long AccountId { get; init; }
    public required long CurrencyId { get; init; }
}