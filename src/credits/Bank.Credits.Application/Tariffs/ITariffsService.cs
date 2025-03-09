using Bank.Common.Application.Z1all.ExecutionResult.StatusCode;
using Bank.Credits.Application.Tariffs.Models;
using X.PagedList;

namespace Bank.Credits.Application.Tariffs
{
    public interface ITariffsService
    {
        Task<ExecutionResult<IPagedList<TariffDto>>> GetTariffsAsync(TariffsFilters filters, int page, int pageSize);
        Task<ExecutionResult<TariffDto>> CreateTariffAsync(CreateTariffDto model);
        Task<ExecutionResult> DeleteTariffAsync(Guid tariffId);
    }
}
