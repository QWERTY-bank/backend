using Bank.Common.Application.Z1all.ExecutionResult.StatusCode;
using Bank.Credits.Application.User.Models;
using Bank.Credits.Domain.User;

namespace Bank.Credits.Application.User
{
    public interface IUserService
    {
        Task<UserEntity> GetUserEntityAsync(Guid userId);
        Task<ExecutionResult<UserCreditInfoDto>> GetUserCreditInfoAsync(Guid userId);
        Task<ExecutionResult> RecalculateRating(Guid userId);
    }
}
