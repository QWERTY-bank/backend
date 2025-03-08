using Bank.Users.Domain.Users;

namespace Bank.Users.Application.Users.Models
{
    public class CreateUserDto
    {
        public required string FullName { get; init; }
        public required DateOnly Birthday { get; init; }
        public required GenderType Gender { get; init; }
        public required string Phone { get; init; }
        public required string Password { get; init; }
    }
}
