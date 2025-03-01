using Bank.Core.Domain.Currencies;

namespace Bank.Core.Domain.Transactions;

public record TransactionCurrency
{
    public required decimal Value { get; init; }
    public required CurrencyCode Code { get; init; }
}