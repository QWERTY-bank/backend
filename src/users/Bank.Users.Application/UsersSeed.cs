using Bank.Common.Models.Auth;
using Bank.Users.Application.Auth;
using Bank.Users.Application.Users;
using Bank.Users.Domain.Users;

namespace Bank.Users.Persistence
{
    public class UsersSeed
    {
        private readonly UsersDbContext _context;
        private readonly IPasswordService _passwordService;
        private readonly IUserService _userService;

        public UsersSeed(
            UsersDbContext context,
            IPasswordService passwordService,
            IUserService userService)
        {
            _context = context;
            _passwordService = passwordService;
            _userService = userService;
        }

        public void SeedUsers()
        {
            UserEntity newUser = new()
            {
                Id = Guid.Parse("13A6CEB6-9C7E-409B-82E6-6D5A78FE5E4D"),
                Phone = "+70987654321",
                FullName = "Сотрудник",
                Birthday = new(1994, 3, 1),
                Gender = GenderType.male,
                IsBlocked = false,
                PasswordHash = _passwordService.HashPassword("stringA1").Result
            };
            AddUser(newUser, [RoleType.Default, RoleType.Employee]);
        }

        private void AddUser(UserEntity newUser, RoleType[] roles)
        {
            var userExist = _context.Users.Any(x => x.Id == newUser.Id);
            if (userExist)
            {
                return;
            }

            _context.Users.Add(newUser);

            foreach (var role in roles)
            {
                _userService.AddUserToRoleAsync(newUser, role).Wait();
            }

            _context.SaveChanges();
        }
    }
}
