using Z1all.ExecutionResult.StatusCode;

namespace Bank.Credits.Application.Requests
{
    // TODO: Добавить сервис для запроса токенов из UsersService
    public class TokenService : ITokenService
    {
        public Task<ExecutionResult<string>> GetServiceTokenAsync()
        {
            throw new NotImplementedException();
        }
    }
}
