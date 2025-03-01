using Bank.Core.Domain.Common;

namespace Bank.Core.Domain.Currencies;

public class CurrencyEntity : BaseEntity<long>
{
    public required CurrencyCode Code { get; set; }
}