using Bank.Credits.Application.Requests.Models;
using Z1all.ExecutionResult.StatusCode;

namespace Bank.Credits.Application.Requests
{
    // TODO: Добавить запросы к CoreService
    public class CoreRequestService : ICoreRequestService
    {
        private readonly ITokenService _tokenService;

        public CoreRequestService(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        public Task<ExecutionResult<BalanceDto>> GetAccountBalanceAsync(long accountId)
        {
            throw new NotImplementedException();
        }

        public Task<ExecutionResult> UnitAccountDepositTransfer(TransactionDto model)
        {
            throw new NotImplementedException();
        }

        public Task<ExecutionResult> UnitAccountWithdrawTransferAsync(TransactionDto model)
        {
            throw new NotImplementedException();
        }
    }
}
