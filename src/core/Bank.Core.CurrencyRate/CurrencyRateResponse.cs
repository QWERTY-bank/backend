using System.Text.Json.Serialization;

namespace Bank.Core.CurrencyRate;

public class CurrencyRateResponse
{
    [JsonPropertyName("Valute")]
    public required Currencies Valute { get; init; }
}

public class Currencies
{
    [JsonPropertyName("USD")]
    public required Currency Usd { get; init; }
    
    [JsonPropertyName("EUR")]
    public required Currency Eur { get; init; }
}

public class Currency
{
    [JsonPropertyName("Nominal")]
    public int Nominal { get; init; }
    
    [JsonPropertyName("Value")]
    public decimal Value { get; init; }
}