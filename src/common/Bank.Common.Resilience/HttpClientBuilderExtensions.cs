using Bank.Common.Resilience.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Logging;
using Polly;

namespace Bank.Common.Resilience;

public static class HttpClientBuilderExtensions
{
    /// <summary>
    /// Применяет политики в порядке: retry -> circuit breaker -> timeout
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="resilience"></param>
    /// <typeparam name="TMarker"></typeparam>
    /// <returns></returns>
    public static IHttpClientBuilder ApplyResilience<TMarker>(
        this IHttpClientBuilder builder,
        ResilienceConfiguration resilience)
    {
        IAsyncPolicy<HttpResponseMessage>? circuitBreakerPolicy = null;

        return builder
            .AddHttpMessageHandler(
                sp =>
                {
                    var logger = sp.GetRequiredService<ILogger<TMarker>>();

                    if (resilience.DurationOfBreak > TimeSpan.Zero && resilience.EventsBeforeBreak > 0)
                    {
                        circuitBreakerPolicy ??= PolicyHelper.CreateCircuitBreakerPolicy(
                            resilience.EventsBeforeBreak,
                            resilience.DurationOfBreak,
                            logger);
                    }

                    var policy = CreateResiliencePolicy(resilience, circuitBreakerPolicy, logger) ??
                                 Policy.NoOpAsync<HttpResponseMessage>();
                    return new PolicyHttpMessageHandler(policy);
                });
    }
    
    private static IAsyncPolicy<HttpResponseMessage>? CreateResiliencePolicy(
        ResilienceConfiguration resilienceConfiguration,
        IAsyncPolicy<HttpResponseMessage>? circuitBreakerPolicy,
        ILogger logger)
    {
        IAsyncPolicy<HttpResponseMessage>? policy = null;

        if (resilienceConfiguration.RetryCount > 0)
        {
            var retryPolicy = PolicyHelper.CreateRetryPolicy(resilienceConfiguration.RetryCount, logger);
            policy = policy != null ? policy.WrapAsync(retryPolicy) : retryPolicy;
        }

        if (circuitBreakerPolicy != null)
        {
            policy = policy != null ? policy.WrapAsync(circuitBreakerPolicy) : circuitBreakerPolicy;
        }

        if (resilienceConfiguration.Timeout > TimeSpan.Zero)
        {
            var timeoutPolicy = Policy.TimeoutAsync<HttpResponseMessage>(
                resilienceConfiguration.Timeout,
                (_, span, _) =>
                {
                    logger.LogDebug("Timeout {Timeout} caught", span);
                    return Task.CompletedTask;
                });

            policy = policy != null ? policy.WrapAsync(timeoutPolicy) : timeoutPolicy;
        }

        return policy;
    }
}