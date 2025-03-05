using Bank.Credits.Application.Credits.Models;
using X.PagedList;
using Z1all.ExecutionResult.StatusCode;

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
