using Bank.Credits.Application.Requests.Models;
using Z1all.ExecutionResult.StatusCode;

namespace Bank.Credits.Application.Requests
{
    public interface ICoreRequestService
    {
        Task<ExecutionResult<BalanceDto>> GetAccountBalanceAsync(long accountId);
        Task<ExecutionResult> UnitAccountDepositTransferAsync(TransactionDto model, long accountId);
        Task<ExecutionResult> UnitAccountWithdrawTransferAsync(TransactionDto model, long accountId);
    }
}
