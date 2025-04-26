using Bank.Common.Resilience.Internal;
using Microsoft.AspNetCore.Builder;

namespace Bank.Common.Resilience;

public static class ErrorMiddlewareExtensions
{
    public static void UseErrorMiddleware(this WebApplication webApplication)
    {
        webApplication.UseMiddleware<ErrorMiddleware>();
    }
}