using Bank.Users.Domain.Enums;

namespace Bank.Users.Application.Users
{
    public class UserDto
    {
        public required Guid Id { get; init; }
        public required string FullName { get; init; }
        public required DateOnly Birthday { get; init; }
        public required Gender Gender { get; init; }
        public required string Phone { get; init; }
    }
}
