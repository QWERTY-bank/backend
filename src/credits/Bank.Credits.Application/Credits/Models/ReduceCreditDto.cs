namespace Bank.Credits.Application.Credits.Models
{
    public class ReduceCreditDto
    {
        public required Guid Key { get; init; }
        public required long AccountId { get; set; }
        public required decimal Value { get; set; }
    }
}
