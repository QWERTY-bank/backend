using Bank.Users.Domain.Common;

namespace Bank.Users.Domain.Users
{
    public class UserEntity : BaseEntity
    {
        public required string FullName { get; set; }
        public DateOnly Birthday { get; set; }
        public GenderType Gender { get; set; }
        public required string Phone { get; set; }
    }
}
