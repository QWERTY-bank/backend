using System.Net;

namespace Bank.Core.Api.Infrastructure.Web;

public sealed class CustomServerError<TValue> :
    IResult,
    IStatusCodeHttpResult,
    IValueHttpResult,
    IValueHttpResult<TValue>
{
    public int? StatusCode { get; }

    object? IValueHttpResult.Value => Value;

    public TValue? Value { get; }

    public CustomServerError(HttpStatusCode httpStatusCode, TValue? value)
    {
        Value = value;
        StatusCode = (int)httpStatusCode;
    }

    public Task ExecuteAsync(HttpContext httpContext)
    {
        ArgumentNullException.ThrowIfNull(httpContext, nameof (httpContext));
        httpContext.Response.StatusCode = StatusCode!.Value;

        if (Value == null)
        {
            return Task.CompletedTask;
        }

        if (typeof (TValue).IsValueType)
        {
            return httpContext.Response.WriteAsJsonAsync<TValue>(Value);
        }

        var type = Value.GetType();
        return httpContext.Response.WriteAsJsonAsync(Value, type);
    }
}
