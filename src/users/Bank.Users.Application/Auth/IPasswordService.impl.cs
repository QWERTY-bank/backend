using Z1all.ExecutionResult;

namespace Bank.Users.Application.Auth
{
    // TODO: Найти библиотеку для паролей
    public class PasswordService : IPasswordService
    {
        public Task<ExecutionResult<string>> HashPassword(string password)
        {
            return Task.FromResult<ExecutionResult<string>>(password);
        }

        public Task<ExecutionResult> CheckPassword(string password, string passwordHash)
        {
            return Task.FromResult(password == passwordHash
                ? ExecutionResult.FromSuccess()
                : ExecutionResult.FromError("CheckPassword", "Invalid password"));
        }
    }
}
