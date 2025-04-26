using System.Collections.Immutable;
using System.Net;
using Bank.Common.Api.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Bank.Common.Resilience.Internal;

internal class ErrorMiddleware
{
    private readonly ILogger<ErrorMiddleware> _logger;
    private readonly RequestDelegate _next;

    public ErrorMiddleware(
        RequestDelegate next,
        ILogger<ErrorMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }
    
    public async Task InvokeAsync(HttpContext httpContext)
    {
        var currentMinute = DateTime.Now.Minute;
        
        double errorProbability = currentMinute % 2 == 0 ? 0.9 : 0.5;
        var randomValue = Random.Shared.NextDouble();

        if (randomValue <= errorProbability)
        {
            await _next(httpContext);
        }
        else
        {
            var status = (int)HttpStatusCode.InternalServerError;
            
            httpContext.Response.StatusCode = status;
            httpContext.Response.ContentType = "application/json";

            ErrorResponse error = new()
            {
                Status = status,
                Title = "Unknow error(s)",
                Errors = ImmutableDictionary<string, List<string>>.Empty.Add("UnknowError", ["Unknown error"]),
            };

            await httpContext.Response.WriteAsJsonAsync(error);
        }
    }
}