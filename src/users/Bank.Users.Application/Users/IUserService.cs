using Bank.Common.Application.Z1all.ExecutionResult.StatusCode;
using Bank.Common.Models.Auth;
using Bank.Users.Application.Users.Models;
using Bank.Users.Domain.Users;
using X.PagedList;

namespace Bank.Users.Application.Users
{
    public interface IUserService
    {
        Task<ExecutionResult<UserDto>> GetUserAsync(Guid userId);
        Task<ExecutionResult<IPagedList<UserShortDto>>> GetUsersAsync(int page, int pageSize);
        Task<bool> AddUserToRoleAsync(UserEntity user, RoleType roleType);
        Task<ExecutionResult> ChangeUserBlockStatusAsync(bool isBlock, Guid userId);
        Task<ExecutionResult> ChangePhoneAsync(ChangePhoneDto model, Guid userId);
        Task<ExecutionResult> ChangePasswordAsync(ChangePasswordDto model, Guid userId);
        Task<ExecutionResult> ChangeProfileAsync(ChangeProfileDto model, Guid userId);
    }
}
