using Z1all.ExecutionResult;

namespace Bank.Users.Application.Auth
{
    public interface IPasswordService
    {
        ExecutionResult<string> HashPassword(string password);
        ExecutionResult CheckPassword(string password, string passwordHash);
    }
}
