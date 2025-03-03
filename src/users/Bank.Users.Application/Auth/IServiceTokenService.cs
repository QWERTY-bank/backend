using Bank.Users.Application.Auth.Models;
using Z1all.ExecutionResult.StatusCode;

namespace Bank.Users.Application.Auth
{
    public interface IServiceTokenService
    {
        ExecutionResult<ServiceTokenDto> CreateServiceTokens();
    }
}
