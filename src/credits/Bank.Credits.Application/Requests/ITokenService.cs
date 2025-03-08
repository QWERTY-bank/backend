using Bank.Common.Application.Z1all.ExecutionResult.StatusCode;

namespace Bank.Credits.Application.Requests
{
    public interface ITokenService
    {
        Task<ExecutionResult<string>> GetServiceTokenAsync();
    }
}
