using Bank.Users.Application.Users.Models;
using X.PagedList;
using Z1all.ExecutionResult.StatusCode;

namespace Bank.Users.Application.Users
{
    public interface IUserService
    {
        Task<ExecutionResult<UserDto>> GetUserAsync(Guid userId);
        Task<ExecutionResult<IPagedList<UserShortDto>>> GetUsersAsync(int page, int pageSize);
        Task<ExecutionResult> ChangeUserBlockStatusAsync(bool isBlock, Guid userId);
        Task<ExecutionResult> ChangePhoneAsync(ChangePhoneDto model, Guid userId);
        Task<ExecutionResult> ChangePasswordAsync(ChangePasswordDto model, Guid userId);
        Task<ExecutionResult> ChangeProfileAsync(ChangeProfileDto model, Guid userId);
    }
}
