using System.Net;
using Bank.Core.Domain.Common;

namespace Bank.Core.Api.Infrastructure.Web;

internal class ErrorProblemDetails : HttpValidationProblemDetails
{
    public ErrorProblemDetails(Error error, HttpStatusCode httpStatusCode)
    {
        ArgumentNullException.ThrowIfNull(error);

        Type = error.Code;
        Detail = error.Message;
        Status = (int)httpStatusCode;
    }
    
    public static ErrorProblemDetails FromError(Error error, HttpStatusCode httpStatusCode) =>
        new(error, httpStatusCode);
}
