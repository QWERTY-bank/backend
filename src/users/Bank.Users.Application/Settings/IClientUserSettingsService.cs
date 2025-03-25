using Bank.Common.Application.Z1all.ExecutionResult.StatusCode;
using Bank.Users.Application.Settings.Models;

namespace Bank.Users.Application.Settings
{
    public interface IClientUserSettingsService
    {
        Task<ExecutionResult<ClientUserSettingsDto>> GetClientUserSettingsAsync(Guid userId);
        Task<ExecutionResult> ToggleNightThemeAsync(bool onNightTheme, Guid userId);
        Task<ExecutionResult> ToggleAccountAsync(bool showAccount, long accountId, Guid userId);
    }
}
