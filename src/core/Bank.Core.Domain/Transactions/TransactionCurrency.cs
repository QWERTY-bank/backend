using Bank.Core.Domain.Currencies;

namespace Bank.Core.Domain.Transactions;

public class TransactionCurrency
{
    public required decimal Value { get; init; }
    public required CurrencyCode Type { get; init; }
}