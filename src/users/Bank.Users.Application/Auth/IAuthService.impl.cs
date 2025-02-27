using Bank.Common.Models.Auth;
using Bank.Users.Application.Auth.Models;
using Bank.Users.Domain.Users;
using Bank.Users.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Z1all.ExecutionResult.StatusCode;

namespace Bank.Users.Application.Auth
{
    public class AuthService : IAuthService
    {
        private readonly ITokensService _tokensService;
        private readonly IPasswordService _passwordService;
        private readonly UsersDbContext _context;
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            ITokensService tokensService,
            IPasswordService passwordService,
            UsersDbContext context,
            ILogger<AuthService> logger)
        {
            _tokensService = tokensService;
            _passwordService = passwordService;
            _context = context;
            _logger = logger;
        }

        public async Task<ExecutionResult<TokensDTO>> RegistrationAsync(RegistrationDTO registrationDTO)
        {
            var userIsExist = await _context.Users.AnyAsync(x => x.Phone == registrationDTO.Phone);
            if (userIsExist)
            {
                return ExecutionResult<TokensDTO>.FromBadRequest("UserIsExist", "User with this phone number already exist.");
            }
 
            var hashPasswordResult = await _passwordService.HashPassword(registrationDTO.Password);
            if (hashPasswordResult.IsNotSuccess)
            {
                _logger.LogError($"Password hashing error");
                return ExecutionResult<TokensDTO>.FromInternalServer("RegistrationFail", "Unknow error.");
            }

            UserEntity newUser = new()
            {
                Phone = registrationDTO.Phone,
                FullName = registrationDTO.FullName,
                Birthday = registrationDTO.Birthday,
                Gender = registrationDTO.Gender,
                PasswordHash = hashPasswordResult.Result
            };

            await _context.Users.AddAsync(newUser);

            var addingToRoleResult = await AddUserToRoleAsync(newUser, RoleType.Default);
            if (!addingToRoleResult)
            {
                _logger.LogCritical($"Role '{RoleType.Default.ToString()}' does not found.");
                return ExecutionResult<TokensDTO>.FromInternalServer("RegistrationFail", "Unknow error.");
            }

            return await _tokensService.CreateTokensAsync(newUser);
        }

        private async Task<bool> AddUserToRoleAsync(UserEntity user, RoleType roleType)
        {
            var role = await _context.Roles.FirstOrDefaultAsync(x => x.Type == roleType);
            if (role == null)
            {
                return false;
            }

            user.Roles ??= new();
            user.Roles.Add(role);

            return true;
        }

        public async Task<ExecutionResult<TokensDTO>> LoginAsync(LoginDTO login)
        {
            var user = await _context.Users
                .Include(x => x.Roles)
                .FirstOrDefaultAsync(x => x.Phone == login.Phone);
            if (user == null)
            {
                _logger.LogInformation($"The user was not found on the phone.");
                return ExecutionResult<TokensDTO>.FromBadRequest("LoginFail", "login fail.");
            }

            var checkingPasswordResult = await _passwordService.CheckPassword(login.Password, user.PasswordHash);
            if (checkingPasswordResult.IsNotSuccess)
            {
                _logger.LogInformation($"Invalid password for user with id = '{user.Id}'.");
                return ExecutionResult<TokensDTO>.FromBadRequest("LoginFail", "login fail.");
            }

            return await _tokensService.CreateTokensAsync(user);
        }

        public async Task<ExecutionResult<TokensDTO>> UpdateAccessTokenAsync(string refresh, Guid accessTokenJTI, Guid userId)
        {
            var removingResult = await _tokensService.RemoveRefreshAsync(refresh, accessTokenJTI);
            if (removingResult.IsNotSuccess)
            {
                return ExecutionResult<TokensDTO>.FromError(removingResult);
            }

            var user = await _context.Users
                .Include(x => x.Roles)
                .FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null)
            {
                _logger.LogInformation($"The user with id {userId} could not be found for unknown reasons.");
                return ExecutionResult<TokensDTO>.FromInternalServer("UpdateAccessTokenFail", "Unknow error.");
            }

            return await _tokensService.CreateTokensAsync(user);
        }
    }
}
