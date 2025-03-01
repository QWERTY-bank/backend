using Bank.Core.Domain.Common;
using Bank.Core.Domain.Currencies;

namespace Bank.Core.Domain.Accounts;

public class AccountCurrencyEntity : BaseEntity<long>
{
    public required decimal Value { get; set; }
    public required long AccountId { get; init; }
    public required CurrencyCode Code { get; init; }
}