using System.Net;
using Bank.Core.Api.Infrastructure.Web;
using Bank.Core.Domain.Common;

namespace Bank.Core.Api.Infrastructure.Extensions;

internal static class EndpointResultExtensions
{
    public static IResult ToEndpointResult<T>(this OperationResult<T> result)
    {
        if (!result.IsError)
        {
            return result.Value is Empty
                ? TypedResults.NoContent()
                : TypedResults.Ok(result.Value);
        }

        return FromError(result.Error!);
    }
    
    private static IResult FromError(Error error)
    {
        switch (error.Code)
        {
            case OperationErrorCodes.NotFound:
                return TypedResults.NotFound(ErrorProblemDetails.FromError(error, HttpStatusCode.BadRequest));
            case OperationErrorCodes.InvalidData:
                return TypedResults.BadRequest(ErrorProblemDetails.FromError(error, HttpStatusCode.BadRequest));
            case OperationErrorCodes.Forbidden:
                return new CustomServerError<ErrorProblemDetails>(
                    HttpStatusCode.Forbidden,
                    ErrorProblemDetails.FromError(error, HttpStatusCode.Forbidden));
            default:
            {
                throw new NotSupportedException($"\"{error.Code}\" processing is not implemented");
            }
        }
    }
}