using Bank.Core.HttpClient.CurrencyRate;
using Refit;

namespace Bank.Core.Api.Infrastructure.Extensions;

public static class CurrencyRateClientExtensions
{
    public static IServiceCollection AddCurrencyRateClient(this IServiceCollection services, IConfiguration configuration)
    {
        var baseUrl = configuration.GetValue<string>("CurrencyRate:Url")!;

        services.AddRefitClient<ICurrencyRateClient>()
            .ConfigureHttpClient(client => { client.BaseAddress = new Uri(baseUrl); });

        return services;
    }
}