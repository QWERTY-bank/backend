using Bank.Common.Application.Z1all.ExecutionResult.StatusCode;
using Bank.Users.Application.Auth.Models;
using Bank.Users.Domain.Users;

namespace Bank.Users.Application.Auth
{
    public interface ITokensService
    {
        Task<ExecutionResult<TokensDTO>> CreateUserTokensAsync(UserEntity user);
        Task<ExecutionResult> RemoveRefreshAsync(Guid accessTokenJTI);
        Task<ExecutionResult> RemoveRefreshAsync(string refreshToken, Guid accessTokenJTI);
    }
}
