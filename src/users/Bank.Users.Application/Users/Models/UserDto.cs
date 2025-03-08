using Bank.Common.Models.Auth;
using Bank.Users.Domain.Users;

namespace Bank.Users.Application.Users.Models
{
    public class UserDto
    {
        public required Guid Id { get; init; }
        public required string FullName { get; init; }
        public required DateOnly Birthday { get; init; }
        public required GenderType Gender { get; init; }
        public required string Phone { get; init; }
        public required bool IsBlocked { get; set; }
        public required RoleType MaxRole { get; set; }
    }
}
