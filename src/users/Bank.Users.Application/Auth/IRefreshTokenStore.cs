namespace Bank.Users.Application.Auth
{
    public interface IRefreshTokenStore
    {
        Task<bool> SaveAsync(string refreshToken, Guid accessTokenJTI, TimeSpan refreshTimeLife);
        Task<bool> RemoveAsync(Guid accessTokenJTI);
        Task<bool> Exist(string refreshToken, Guid accessTokenJTI);
    }
}
