using Bank.Common.Application.Models;
using Z1all.ExecutionResult.StatusCode;

namespace Bank.Users.Application.Auth
{
    public interface IServiceTokenService
    {
        ExecutionResult<ServiceTokenDto> CreateServiceTokens();
    }
}
