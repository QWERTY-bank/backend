using Bank.Credits.Application.Tariffs.Models;
using X.PagedList;
using Z1all.ExecutionResult.StatusCode;

namespace Bank.Credits.Application.Tariffs
{
    public interface ITariffsService
    {
        Task<ExecutionResult<IPagedList<TariffDto>>> GetTariffsAsync(TariffsFilters filters, int page, int pageSize);
        Task<ExecutionResult> CreateTariffAsync(CreateTariffDto model);
        Task<ExecutionResult> DeleteTariffAsync(Guid tariffId);
    }
}
