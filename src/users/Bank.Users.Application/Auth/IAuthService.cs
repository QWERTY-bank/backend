using Bank.Users.Application.Auth.Models;
using Z1all.ExecutionResult.StatusCode;

namespace Bank.Users.Application.Auth
{
    public interface IAuthService
    {
        Task<ExecutionResult<TokensDTO>> RegistrationAsync(RegistrationDTO registrationDTO);
        Task<ExecutionResult<TokensDTO>> LoginAsync(LoginDTO loginDTO);
        Task<ExecutionResult<TokensDTO>> UpdateAccessTokenAsync(string refresh, Guid accessTokenJTI, Guid userId);
    }
}
