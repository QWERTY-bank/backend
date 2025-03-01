using Bank.Common.Models.Auth;
using Bank.Users.Domain.Common;

namespace Bank.Users.Domain.Users
{
    public class Role : BaseEntity
    {
        public required RoleType Type { get; set; }
        public required string RoleName { get; set; }
    }
}
