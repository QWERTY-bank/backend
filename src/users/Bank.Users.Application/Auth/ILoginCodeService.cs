using Bank.Common.Application.Z1all.ExecutionResult.StatusCode;
using Bank.Users.Application.Auth.Models;

namespace Bank.Users.Application.Auth
{
    public interface ILoginCodeService
    {
        Task<ExecutionResult<LoginCodeDto>> CreateUserLoginCodeAsync(Guid userId);
        Task<ExecutionResult<Guid>> GetUserIdAsync(LoginCodeDto loginCode);
    }
}
