using AutoMapper;
using Bank.Common.Application.Z1all.ExecutionResult.StatusCode;
using Bank.Users.Application.Settings.Models;
using Bank.Users.Domain.Settings;
using Bank.Users.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Bank.Users.Application.Settings
{
    public class ClientUserSettingsService : IClientUserSettingsService
    {
        private readonly UsersDbContext _context;
        private readonly ILogger<ClientUserSettingsService> _logger;
        private readonly IMapper _mapper;

        public ClientUserSettingsService(
            UsersDbContext context,
            ILogger<ClientUserSettingsService> logger,
            IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ExecutionResult<ClientUserSettingsDto>> GetClientUserSettingsAsync(Guid userId)
        {
            var settings = await _context.ClientUserSettings
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (settings == null)
            {
                settings = new()
                {
                    HiddenAccounts = [],
                    IsNightTheme = false,
                };
            }

            return _mapper.Map<ClientUserSettingsDto>(settings);
        }

        public async Task<ExecutionResult> ToggleNightThemeAsync(bool onNightTheme, Guid userId)
        {
            return await HandleSettingsUpdateAsync(userId, settings =>
            {
                settings.IsNightTheme = onNightTheme;
            });
        }

        public async Task<ExecutionResult> ToggleAccountAsync(bool showAccount, long accountId, Guid userId)
        {
            return await HandleSettingsUpdateAsync(userId, settings =>
            {
                if (showAccount)
                {
                    settings.HiddenAccounts = settings.HiddenAccounts
                        .Where(x => x != accountId)
                        .ToArray();
                }
                else
                {
                    settings.HiddenAccounts = settings.HiddenAccounts
                        .Where(x => x != accountId)
                        .Append(accountId)
                        .ToArray();
                }
            });
        }

        private async Task<ExecutionResult> HandleSettingsUpdateAsync(Guid userId, Action<ClientUserSettings> updateSettings)
        {
            var settings = await _context.ClientUserSettings
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (settings == null)
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(x => x.Id == userId);

                if (user == null)
                {
                    return ExecutionResult.FromNotFound("UserNotFound", $"User with id {userId} does not found.");
                }

                settings = new()
                {
                    HiddenAccounts = [],
                    IsNightTheme = false,
                    UserId = userId,
                };

                await _context.ClientUserSettings.AddAsync(settings);
            }

            updateSettings(settings);

            await _context.SaveChangesAsync();

            return ExecutionResult.FromSuccess();
        }
    }
}
