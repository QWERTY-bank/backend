using Bank.Core.Domain.Currencies;

namespace Bank.Core.Application.Currencies.Models;

public class CurrencyRateDto
{
    public CurrencyCode Code { get; init; }
    public decimal RubValue { get; init; }
}