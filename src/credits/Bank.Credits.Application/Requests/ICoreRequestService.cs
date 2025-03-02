using Bank.Credits.Application.Requests.Models;
using Z1all.ExecutionResult.StatusCode;

namespace Bank.Credits.Application.Requests
{
    public interface ICoreRequestService
    {
        Task<ExecutionResult<BalanceDto>> GetAccountBalanceAsync(long accountId);
        Task<ExecutionResult> UnitAccountDepositTransfer(TransactionDto model);
        Task<ExecutionResult> UnitAccountWithdrawTransferAsync(TransactionDto model);
    }
}
