using Bank.Common.Application.Z1all.ExecutionResult.StatusCode;

namespace Bank.Users.Application.Auth
{
    public class PasswordService : IPasswordService
    {
        public ExecutionResult<string> HashPassword(string password) 
            => BCrypt.Net.BCrypt.HashPassword(password);

        public ExecutionResult CheckPassword(string password, string passwordHash) 
            => BCrypt.Net.BCrypt.Verify(password, passwordHash)
                ? ExecutionResult.FromSuccess()
                : ExecutionResult.FromBadRequest("CheckPassword", "Invalid password");
    }
}
