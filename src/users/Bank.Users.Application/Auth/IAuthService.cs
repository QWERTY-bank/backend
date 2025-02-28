using Bank.Common.Models.Auth;
using Bank.Users.Application.Auth.Models;
using Z1all.ExecutionResult.StatusCode;

namespace Bank.Users.Application.Auth
{
    public interface IAuthService
    {
        Task<ExecutionResult<TokensDTO>> RegistrationAsync(RegistrationDTO model, RoleType role = RoleType.Default);
        Task<ExecutionResult<TokensDTO>> LoginAsync(LoginDTO model);
        Task<ExecutionResult<TokensDTO>> UpdateAccessTokenAsync(string refresh, Guid accessTokenJTI, Guid userId);
    }
}
