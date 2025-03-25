using Bank.Users.Domain.Common;
using Bank.Users.Domain.Users;

namespace Bank.Users.Domain.Settings
{
    public class ClientUserSettings : BaseEntity
    {
        public bool IsNightTheme { get; set; }

        public long[] HiddenAccounts { get; set; } = null!;

        public Guid UserId { get; set; }
        public UserEntity User { get; set; }
    }
}
