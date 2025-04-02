using Bank.Core.Application.Currencies.Models;
using Bank.Core.Domain.Currencies;

namespace Bank.Core.Application.Abstractions;

public interface ICurrencyRateService
{
    Task<decimal> ConvertCurrency(
        decimal amount,
        CurrencyCode fromCurrency,
        CurrencyCode toCurrency,
        CancellationToken cancellationToken);
    
    Task<IReadOnlyCollection<CurrencyRateDto>> GetCurrencyRate(
        IReadOnlyCollection<CurrencyCode> codes,
        CancellationToken cancellationToken);
}