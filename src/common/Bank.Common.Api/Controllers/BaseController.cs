using Bank.Common.Api.Attributes;
using Bank.Common.Api.DTOs;
using Bank.Common.Auth.Extensions;
using Microsoft.AspNetCore.Mvc;
using Z1all.ExecutionResult.StatusCode;

namespace Bank.Common.Api.Controllers
{
    [ApiController]
    [ValidateModelState]
    public abstract class BaseController : ControllerBase
    {
        protected Guid UserId { get => User.GetUserId(); }

        protected Guid TokenJTI { get => User.GetTokenId(); }

        protected async Task<ActionResult> ExecutionResultHandlerAsync(Func<Task<ExecutionResult>> operation)
        {
            ExecutionResult response = await operation();

            return ExecutionResultHandler(response);
        }

        protected async Task<ActionResult<TResult>> ExecutionResultHandlerAsync<TResult>(Func<Task<ExecutionResult<TResult>>> operation)
        {
            ExecutionResult<TResult> response = await operation();

            if (!response.IsSuccess) return ExecutionResultHandler(ExecutionResult.FromError(response));
            return Ok(response.Result!);
        }

        private ObjectResult ExecutionResultHandler(ExecutionResult executionResult, string? otherMassage = null)
        {
            return StatusCode((int)executionResult.StatusCode, new ErrorResponse()
            {
                Title = otherMassage ?? "One or more errors occurred.",
                Status = (int)executionResult.StatusCode,
                Errors = executionResult.Errors,
            });
        }
    }
}
