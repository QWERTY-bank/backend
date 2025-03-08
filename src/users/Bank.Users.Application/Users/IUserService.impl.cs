using AutoMapper;
using Bank.Common.Application.Extensions;
using Bank.Common.Application.Z1all.ExecutionResult.StatusCode;
using Bank.Common.Models.Auth;
using Bank.Users.Application.Auth;
using Bank.Users.Application.Auth.Models;
using Bank.Users.Application.Users.Models;
using Bank.Users.Domain.Users;
using Bank.Users.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using X.PagedList;

namespace Bank.Users.Application.Users
{
    public class UserService : IUserService
    {
        private readonly UsersDbContext _context;
        private readonly IPasswordService _passwordService;
        private readonly ILogger<UserService> _logger;
        private readonly IMapper _mapper;

        public UserService(
            UsersDbContext context,
            IPasswordService passwordService,
            ILogger<UserService> logger,
            IMapper mapper)
        {
            _context = context;
            _passwordService = passwordService;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ExecutionResult> CreateUserAsync(CreateUserDto model, RoleType roleType)
        {
            var userIsExist = await _context.Users.AnyAsync(x => x.Phone == model.Phone);
            if (userIsExist)
            {
                return ExecutionResult.FromBadRequest("UserIsExist", "User with this phone number already exist.");
            }

            var hashPasswordResult = _passwordService.HashPassword(model.Password);
            if (hashPasswordResult.IsNotSuccess)
            {
                _logger.LogError($"Password hashing error");
                return ExecutionResult.FromInternalServer("RegistrationFail", "Unknow error.");
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

            var addingToRoleResult = await AddUserToRoleAsync(newUser, roleType);
            if (!addingToRoleResult)
            {
                _logger.LogCritical($"Role '{roleType.ToString()}' does not found.");
                return ExecutionResult.FromInternalServer("CreateUserFail", "Unknow error.");
            }

            if (roleType == RoleType.Employee)
            {
                var addingToDefaultRoleResult = await AddUserToRoleAsync(newUser, RoleType.Default);
                if (!addingToDefaultRoleResult)
                {
                    _logger.LogCritical($"Role '{RoleType.Default.ToString()}' does not found.");
                    return ExecutionResult.FromInternalServer("CreateUserFail", "Unknow error.");
                }
            }

            await _context.SaveChangesAsync();

            return ExecutionResult.FromSuccess();
        }

        public async Task<ExecutionResult<UserDto>> GetUserAsync(Guid userId)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null)
            {
                return ExecutionResult<UserDto>.FromNotFound("UserNotFound", "User not found.");
            }

            return _mapper.Map<UserDto>(user);
        }

        public async Task<ExecutionResult<IPagedList<UserShortDto>>> GetUsersAsync(int page, int pageSize)
        {
            var users = await _context.Users
                .ToPagedListAsync(page, pageSize);

            var result = users.ToMappedPagedList<UserEntity, UserShortDto>(_mapper);

            return ExecutionResult<IPagedList<UserShortDto>>.FromSuccess(result);
        }

        public async Task<bool> AddUserToRoleAsync(UserEntity user, RoleType roleType)
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

        public async Task<ExecutionResult> ChangeUserBlockStatusAsync(bool isBlock, Guid userId)
        {
            return await UpdateUserHandlerAsync(userId, user =>
            {
                user.IsBlocked = isBlock;

                return ExecutionResult.FromSuccess();
            });
        }

        public async Task<ExecutionResult> ChangePasswordAsync(ChangePasswordDto model, Guid userId)
        {
            return await UpdateUserHandlerAsync(userId, user =>
            {
                var checkingResult = _passwordService.CheckPassword(model.CurrentPassword, user.PasswordHash);
                if (checkingResult.IsNotSuccess)
                {
                    return checkingResult;
                }

                var hashResult = _passwordService.HashPassword(model.NewPassword);
                if (hashResult.IsNotSuccess)
                {
                    return ExecutionResult.FromError(hashResult);
                }

                user.PasswordHash = hashResult.Result;

                return ExecutionResult.FromSuccess();
            });
        }

        public async Task<ExecutionResult> ChangePhoneAsync(ChangePhoneDto model, Guid userId)
        {
            return await UpdateUserHandlerAsync(userId, user =>
            {
                user.Phone = model.NewPhone;

                return ExecutionResult.FromSuccess();
            });
        }

        public async Task<ExecutionResult> ChangeProfileAsync(ChangeProfileDto model, Guid userId)
        {
            return await UpdateUserHandlerAsync(userId, user =>
            {
                user.Birthday = model.NewBirthday;
                user.FullName = model.NewFullName;
                user.Gender = model.NewGender;

                return ExecutionResult.FromSuccess();
            });
        }

        private async Task<ExecutionResult> UpdateUserHandlerAsync(Guid userId, Func<UserEntity, ExecutionResult> updater)
        {
            var user = await _context.Users
               .FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null)
            {
                return ExecutionResult.FromNotFound("UserNotFound", "User not found.");
            }

            var result = updater(user);

            await _context.SaveChangesAsync();

            return result;
        }
    }
}
