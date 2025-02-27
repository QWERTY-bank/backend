using Bank.Users.Domain.Common;

namespace Bank.Users.Domain.Users
{
    public class UserEntity : BaseEntity
    {
        public required string FullName { get; set; }
        public required DateOnly Birthday { get; set; }
        public required GenderType Gender { get; set; }
        public required string Phone { get; set; }

        public required string PasswordHash { get; set; }

        public List<Role> Roles { get; set; } = null!;
    }
}
