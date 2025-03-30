using Refit;

namespace Bank.Core.CurrencyRate;

public interface ICurrencyRateClient
{
    [Get("/daily_json.js")]
    Task<CurrencyRateResponse> GetCurrencyRate(CancellationToken cancellationToken);
}