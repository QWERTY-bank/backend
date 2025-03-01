using Bank.Users.Application.Auth.Models;
using Bank.Users.Domain.Users;
using Z1all.ExecutionResult.StatusCode;

namespace Bank.Users.Application.Auth
{
    public interface ITokensService
    {
        Task<ExecutionResult<TokensDTO>> CreateTokensAsync(UserEntity user);
        Task<ExecutionResult> RemoveRefreshAsync(Guid accessTokenJTI);
        Task<ExecutionResult> RemoveRefreshAsync(string refreshToken, Guid accessTokenJTI);
    }
}
