using Bank.Common.Api.Attributes;
using Bank.Common.Api.DTOs;
using Bank.Common.Auth.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;
using Z1all.ExecutionResult.StatusCode;
using Bank.Common.Application.Extensions;
using static Bank.Common.Application.Extensions.PagedListExtensions;

namespace Bank.Common.Api.Controllers
{
    [ApiController]
    [ValidateModelState]
    public abstract class BaseController : ControllerBase
    {
        protected Guid UserId { get => User.GetUserId(); }

        protected Guid TokenJTI { get => User.GetTokenId(); }

        protected async Task<IActionResult> ExecutionResultHandlerAsync(Func<Task<ExecutionResult>> operation)
        {
            ExecutionResult response = await operation();

            return ExecutionResultHandler(response);
        }

        protected async Task<IActionResult> ExecutionResultHandlerAsync<TResult>(Func<Task<ExecutionResult<TResult>>> operation)
        {
            ExecutionResult<TResult> response = await operation();

            if (!response.IsSuccess) return ExecutionResultHandler(ExecutionResult.FromError(response));
            return Ok(response.Result!);
        }

        protected async Task<IActionResult> ExecutionResultHandlerAsync<TResult>(Func<Task<ExecutionResult<IPagedList<TResult>>>> operation)
        {
            ExecutionResult<IPagedList<TResult>> response = await operation();

            if (!response.IsSuccess) return ExecutionResultHandler(ExecutionResult.FromError(response));
            return Ok(response.Result.AddMetaData());
        }

        private ActionResult ExecutionResultHandler(ExecutionResult executionResult, string? otherMassage = null)
        {
            if (executionResult.IsSuccess)
            {
                return StatusCode(StatusCodes.Status204NoContent);
            }

            return StatusCode((int)executionResult.StatusCode, new ErrorResponse()
            {
                Title = otherMassage ?? "One or more errors occurred.",
                Status = (int)executionResult.StatusCode,
                Errors = executionResult.Errors,
            });
        }
    }
}
