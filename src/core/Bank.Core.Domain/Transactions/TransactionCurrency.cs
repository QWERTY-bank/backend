using Bank.Core.Domain.Currencies;

namespace Bank.Core.Domain.Transactions;

public record TransactionCurrency
{
    private const int Decimals = 2;

    private readonly decimal _value;
    
    public required decimal Value
    {
        get => _value;
        init => _value = Math.Round(value, Decimals);
    }
    public required CurrencyCode Code { get; init; }
}