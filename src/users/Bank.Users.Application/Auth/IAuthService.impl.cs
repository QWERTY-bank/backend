using Bank.Common.Application.Z1all.ExecutionResult.StatusCode;
using Bank.Common.Models.Auth;
using Bank.Users.Application.Auth.Models;
using Bank.Users.Application.Users;
using Bank.Users.Domain.Users;
using Bank.Users.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Bank.Users.Application.Auth
{
    public class AuthService : IAuthService
    {
        private readonly ILoginCodeService _loginCodeService;
        private readonly ITokensService _tokensService;
        private readonly IPasswordService _passwordService;
        private readonly IUserService _userService;
        private readonly UsersDbContext _context;
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            ILoginCodeService loginCodeService,
            ITokensService tokensService,
            IPasswordService passwordService,
            IUserService userService,
            UsersDbContext context,
            ILogger<AuthService> logger)
        {
            _loginCodeService = loginCodeService;
            _tokensService = tokensService;
            _passwordService = passwordService;
            _userService = userService;
            _context = context;
            _logger = logger;
        }

        public async Task<ExecutionResult<TokensDTO>> RegistrationAsync(RegistrationDTO model)
        {
            var userIsExist = await _context.Users.AnyAsync(x => x.Phone == model.Phone);
            if (userIsExist)
            {
                return ExecutionResult<TokensDTO>.FromBadRequest("UserIsExist", "User with this phone number already exist.");
            }
 
            var hashPasswordResult = _passwordService.HashPassword(model.Password);
            if (hashPasswordResult.IsNotSuccess)
            {
                _logger.LogError($"Password hashing error");
                return ExecutionResult<TokensDTO>.FromInternalServer("RegistrationFail", "Unknow error.");
            }

            UserEntity newUser = new()
            {
                Phone = model.Phone,
                FullName = model.FullName,
                Birthday = model.Birthday,
                Gender = model.Gender,
                IsBlocked = false,
                PasswordHash = hashPasswordResult.Result
            };

            await _context.Users.AddAsync(newUser);

            var addingToRoleResult = await _userService.AddUserToRoleAsync(newUser, RoleType.Default);
            if (!addingToRoleResult)
            {
                _logger.LogCritical($"Role '{RoleType.Default.ToString()}' does not found.");
                return ExecutionResult<TokensDTO>.FromInternalServer("RegistrationFail", "Unknow error.");
            }

            await _context.SaveChangesAsync();

            return await _tokensService.CreateUserTokensAsync(newUser);
        }

        public async Task<ExecutionResult<TokensDTO>> LoginAsync(LoginDTO model)
        {
            var userResult = await GetUserByLoginAndPasswordAsync(model);
            if (userResult.IsNotSuccess)
            {
                return ExecutionResult<TokensDTO>.FromError(userResult);
            }

            return await _tokensService.CreateUserTokensAsync(userResult.Result);
        }

        public async Task<ExecutionResult<LoginCodeDto>> GetLoginCodeAsync(LoginDTO model)
        {
            var userResult = await GetUserByLoginAndPasswordAsync(model);
            if (userResult.IsNotSuccess)
            {
                return ExecutionResult<LoginCodeDto>.FromError(userResult);
            }

            return await _loginCodeService.CreateUserLoginCodeAsync(userResult.Result.Id);
        }

        public async Task<ExecutionResult<TokensDTO>> LoginAsync(LoginCodeDto model)
        {
            var userIdResult = await _loginCodeService.GetUserIdAsync(model);
            if (userIdResult.IsNotSuccess)
            {
                return ExecutionResult<TokensDTO>.FromError(userIdResult);
            }

            var user = await _context.Users
               .Include(x => x.Roles)
               .FirstOrDefaultAsync(x => x.Id == userIdResult.Result);
            if (user == null)
            {
                _logger.LogInformation($"The user was not found on the phone.");
                return ExecutionResult<TokensDTO>.FromBadRequest("LoginFail", "login fail.");
            }

            if (user.IsBlocked)
            {
                _logger.LogInformation($"User with id = '{user.Id}' is blocked.");
                return ExecutionResult<TokensDTO>.FromForbid("UserBlocked", "The user has been blocked.");
            }

            return await _tokensService.CreateUserTokensAsync(user);
        }

        private async Task<ExecutionResult<UserEntity>> GetUserByLoginAndPasswordAsync(LoginDTO model)
        {
            var user = await _context.Users
               .Include(x => x.Roles)
               .FirstOrDefaultAsync(x => x.Phone == model.Phone);
            if (user == null)
            {
                _logger.LogInformation($"The user was not found on the phone.");
                return ExecutionResult<UserEntity>.FromBadRequest("LoginFail", "login fail.");
            }

            if (user.IsBlocked)
            {
                _logger.LogInformation($"User with id = '{user.Id}' is blocked.");
                return ExecutionResult<UserEntity>.FromForbid("UserBlocked", "The user has been blocked.");
            }

            var checkingPasswordResult = _passwordService.CheckPassword(model.Password, user.PasswordHash);
            if (checkingPasswordResult.IsNotSuccess)
            {
                _logger.LogInformation($"Invalid password for user with id = '{user.Id}'.");
                return ExecutionResult<UserEntity>.FromBadRequest("LoginFail", "login fail.");
            }

            return user;
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

            if (user.IsBlocked)
            {
                _logger.LogInformation($"User with id = '{user.Id}' is blocked.");
                return ExecutionResult<TokensDTO>.FromForbid("UserBlocked", "The user has been blocked.");
            }

            return await _tokensService.CreateUserTokensAsync(user);
        }
    }
}
