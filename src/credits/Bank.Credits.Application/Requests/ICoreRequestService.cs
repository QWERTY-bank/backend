using Bank.Common.Application.Z1all.ExecutionResult.StatusCode;
using Bank.Credits.Application.Requests.Models;

namespace Bank.Credits.Application.Requests
{
    public interface ICoreRequestService
    {
        Task<ExecutionResult<BalanceDto>> GetAccountBalanceAsync(long accountId);
        Task<ExecutionResult> UnitAccountDepositTransferAsync(TransactionDto model, long accountId);
        Task<ExecutionResult> UnitAccountWithdrawTransferAsync(TransactionDto model, long accountId);
    }
}
