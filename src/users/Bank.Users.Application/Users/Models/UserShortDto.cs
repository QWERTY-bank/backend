namespace Bank.Users.Application.Users.Models
{
    public class UserShortDto
    {
        public required Guid Id { get; init; }
        public required string FullName { get; init; }
        public required string Phone { get; init; }
    }
}
