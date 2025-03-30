using System.Text.Json;
using Bank.Core.Application.Abstractions;
using Bank.Core.Application.Currencies.Models;
using Bank.Core.CurrencyRate;
using Bank.Core.Domain.Currencies;
using Microsoft.Extensions.Caching.Distributed;
using RedLockNet;

namespace Bank.Core.Api.Services;

public class CurrencyRateService : ICurrencyRateService
{
    public const string CurrencyRateKey = "currency_rate_key";
    
    private readonly IDistributedCache _cache;
    private readonly IDistributedLockFactory _lockFactory;
    private readonly ICurrencyRateClient _currencyRateClient;

    public CurrencyRateService(
        IDistributedCache cache,
        ICurrencyRateClient currencyRateClient,
        IDistributedLockFactory lockFactory)
    {
        _cache = cache;
        _currencyRateClient = currencyRateClient;
        _lockFactory = lockFactory;
    }
    
    public async Task<decimal> ConvertCurrency(
        decimal amount,
        CurrencyCode fromCurrency,
        CurrencyCode toCurrency,
        CancellationToken cancellationToken)
    {
        var currencyRate = (await GetCurrencyRate(
            [fromCurrency, toCurrency],
            cancellationToken))
            .ToDictionary(rate => rate.Code);
        
        var fromCurrencyRate = currencyRate[fromCurrency];
        var toCurrencyRate = currencyRate[toCurrency];
        
        return amount * fromCurrencyRate.RubValue / toCurrencyRate.RubValue;
    }

    public async Task<IReadOnlyCollection<CurrencyRateDto>> GetCurrencyRate(
        IReadOnlyCollection<CurrencyCode> codes,
        CancellationToken cancellationToken)
    {
        var currencies = await GetCurrenciesFromCache(cancellationToken);
        
        if (currencies == null)
        {
            await using var redLock = await _lockFactory.CreateLockAsync(CurrencyRateKey, TimeSpan.FromSeconds(5));
            
            if (redLock.IsAcquired)
            {
                currencies = await GetCurrenciesFromCache(cancellationToken);
                
                if (currencies == null)
                {
                    var rateResponse = await _currencyRateClient.GetCurrencyRate(cancellationToken);
                        
                    currencies = rateResponse.Valute;
                        
                    var options = new DistributedCacheEntryOptions()
                        .SetAbsoluteExpiration(DateTimeOffset.UtcNow.AddHours(12));
                        
                    await _cache.SetStringAsync(
                        CurrencyRateKey,
                        JsonSerializer.Serialize(currencies),
                        options,
                        cancellationToken);
                }
            }
        }

        if (currencies == null)
        {
            throw new InvalidOperationException("Не удалось получить текущий курс валюты");
        }

        return codes
            .Select(code => GetCurrencyRate(currencies, code))
            .ToArray();
    }
    
    private async Task<Currencies?> GetCurrenciesFromCache(CancellationToken cancellationToken)
    {
        var cachedString = await _cache.GetStringAsync(CurrencyRateKey, cancellationToken);

        return cachedString == null
            ? null
            : JsonSerializer.Deserialize<Currencies>(cachedString);
    }

    private static CurrencyRateDto GetCurrencyRate(Currencies currencies, CurrencyCode code)
    {
        return code switch
        {
            CurrencyCode.Rub => new CurrencyRateDto
            {
                RubValue = 1,
                Code = code
            },
            CurrencyCode.Usd => new CurrencyRateDto
            {
                RubValue = currencies.Usd.Value / currencies.Usd.Nominal,
                Code = code
            },
            CurrencyCode.Eur => new CurrencyRateDto
            {
                RubValue = currencies.Eur.Value / currencies.Eur.Nominal,
                Code = code
            },
            _ => throw new ArgumentOutOfRangeException(nameof(code), code, null)
        };
    }
}