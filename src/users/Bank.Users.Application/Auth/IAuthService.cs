using Bank.Common.Application.Z1all.ExecutionResult.StatusCode;
using Bank.Users.Application.Auth.Models;

namespace Bank.Users.Application.Auth
{
    public interface IAuthService
    {
        Task<ExecutionResult<TokensDTO>> RegistrationAsync(RegistrationDTO model);
        Task<ExecutionResult<TokensDTO>> LoginAsync(LoginDTO model);
        Task<ExecutionResult<TokensDTO>> UpdateAccessTokenAsync(string refresh, Guid accessTokenJTI, Guid userId);
    }
}
