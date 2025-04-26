using System.Net;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Timeout;

namespace Bank.Common.Resilience.Internal;

internal static class PolicyHelper
{
    public static IAsyncPolicy<HttpResponseMessage> CreateRetryPolicy(int retryCount, ILogger logger) =>
        Policy.Handle<Exception>(IsTransientException)
            .OrResult<HttpResponseMessage>(IsTransientErrorResponse)
            .WaitAndRetryAsync(
                retryCount,
                DefaultDurationFn,
                (result, span, attempt, ctx) =>
                {
                    if (result.Result != null)
                    {
                        logger.Log(
                            retryCount == attempt ? LogLevel.Warning : LogLevel.Debug,
                            "Request failed with {StatusCode}. " +
                            "Waiting {TimeSpan} before next retry. " +
                            (retryCount == attempt ? "Last {Attempt} retry attempt" : "Retry attempt {Attempt}"),
                            result.Result.StatusCode,
                            span,
                            attempt);
                    } else if (result.Exception != null)
                    {
                        logger.LogWarning(
                            result.Exception,
                            "Request failed because of exception. " +
                            "Waiting {TimeSpan} before next retry. " +
                            (retryCount == attempt ? "Last {Attempt} retry attempt" : "Retry attempt {Attempt}"),
                            span,
                            attempt);
                    }
                    else
                    {
                        logger.LogWarning(
                            "Request failed because of unknown reason. " +
                            "Waiting {TimeSpan} before next retry. " +
                            (retryCount == attempt ? "Last {Attempt} retry attempt" : "Retry attempt {Attempt}"),
                            span,
                            attempt);
                    }

                    return Task.CompletedTask;
                });

    public static IAsyncPolicy<HttpResponseMessage> CreateCircuitBreakerPolicy(
        int eventsBeforeBreaking,
        TimeSpan durationOfBreak,
        ILogger logger) =>
        Policy.Handle<Exception>(IsTransientException)
            .OrResult<HttpResponseMessage>(IsTransientErrorResponse)
            .CircuitBreakerAsync(
                eventsBeforeBreaking,
                durationOfBreak,
                (_, span) => { logger.LogWarning("Circuit broken for {TimeSpan}", span); },
                () => { logger.LogInformation("Circuit Reset"); },
                () => { logger.LogWarning("Circuit Half-open"); });

    private static bool IsTransientErrorResponse(HttpResponseMessage message) =>
        IsTransientStatusCode(message.StatusCode) ||
        message is { StatusCode: HttpStatusCode.TooManyRequests, Headers.RetryAfter: not null };

    private static TimeSpan DefaultDurationFn(int @try, DelegateResult<HttpResponseMessage> result, Context context)
    {
        if (result.Result?.Headers.RetryAfter?.Delta is not null)
        {
            return result.Result.Headers.RetryAfter.Delta.Value;
        }

        // 500ms ~ 200ms -> 1s ~ 200ms -> 2s ~ 200ms
        return TimeSpan.FromMilliseconds(250 * Math.Pow(2, @try)) +
               TimeSpan.FromMilliseconds(Random.Shared.Next(0, 200));
    }

    private static bool IsTransientException(Exception exception) =>
        exception switch
        {
            HttpRequestException ex => IsTransientStatusCode(ex.StatusCode),
            TaskCanceledException or TimeoutException or TimeoutRejectedException => true,
            _ => false,
        };

    private static bool IsTransientStatusCode(HttpStatusCode? code) =>
        code is HttpStatusCode.RequestTimeout
            or HttpStatusCode.BadGateway
            or HttpStatusCode.ServiceUnavailable
            or HttpStatusCode.GatewayTimeout;
}