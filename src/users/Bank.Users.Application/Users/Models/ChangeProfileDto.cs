using Bank.Users.Domain.Users;

namespace Bank.Users.Application.Users.Models
{
    public class ChangeProfileDto
    {
        public required string NewFullName { get; init; }
        public required DateOnly NewBirthday { get; init; }
        public required GenderType NewGender { get; init; }
    }
}
