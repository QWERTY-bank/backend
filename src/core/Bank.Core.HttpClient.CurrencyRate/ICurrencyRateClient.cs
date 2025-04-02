using Refit;

namespace Bank.Core.HttpClient.CurrencyRate;

public interface ICurrencyRateClient
{
    [Get("/daily_json.js")]
    Task<CurrencyRateResponse> GetCurrencyRate(CancellationToken cancellationToken);
}