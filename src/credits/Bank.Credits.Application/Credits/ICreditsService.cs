using Bank.Common.Application.Z1all.ExecutionResult.StatusCode;
using Bank.Credits.Application.Credits.Models;
using X.PagedList;

namespace Bank.Credits.Application.Credits
{
    public interface ICreditsService
    {
        Task<ExecutionResult<IPagedList<CreditShortDto>>> GetCreditsAsync(CreditsFilter filter, int page, int pageSize, Guid userId);
        Task<ExecutionResult<CreditDto>> GetCreditAsync(Guid creditId, Guid userId);
        Task<ExecutionResult> TakeCreditAsync(TakeCreditDto model, Guid userId);
        Task<ExecutionResult> ReduceCreditAsync(Guid creditId, ReduceCreditDto model, Guid userId);
    }
}
