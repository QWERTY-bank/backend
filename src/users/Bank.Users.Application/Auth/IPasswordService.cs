using Z1all.ExecutionResult;

namespace Bank.Users.Application.Auth
{
    public interface IPasswordService
    {
        Task<ExecutionResult<string>> HashPassword(string password);
        Task<ExecutionResult> CheckPassword(string password, string passwordHash);
    }
}
