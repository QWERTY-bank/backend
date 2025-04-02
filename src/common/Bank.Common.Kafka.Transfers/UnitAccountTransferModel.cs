namespace Bank.Common.Kafka.Transfers;

public class UnitAccountTransferModel
{
    public required UnitTransferType Type { get; init; }
    public required Guid UnitId { get; init; }
    public required long UserAccountId { get; init; }
    public required Guid Key { get; init; }
    public required CurrencyValueModel CurrencyValue { get; init; }
}

public class CurrencyValueModel
{
    public required TransferCurrencyCode Code { get; init; }
    
    public required decimal Value { get; init; }
}

public enum TransferCurrencyCode
{
    Rub,
    Usd,
    Eur
}

public enum UnitTransferType
{
    Deposit,
    Withdraw
}
