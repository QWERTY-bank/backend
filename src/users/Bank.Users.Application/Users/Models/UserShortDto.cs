using Bank.Common.Models.Auth;

namespace Bank.Users.Application.Users.Models
{
    public class UserShortDto
    {
        public required Guid Id { get; init; }
        public required string FullName { get; init; }
        public required string Phone { get; init; }
        public required bool IsBlocked { get; set; }
        public required RoleType MaxRole { get; set; }
    }
}
