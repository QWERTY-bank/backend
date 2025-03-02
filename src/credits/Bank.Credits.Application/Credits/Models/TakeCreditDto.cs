namespace Bank.Credits.Application.Credits.Models
{
    public class TakeCreditDto
    {
        public required Guid Key { get; init; }
        public required long AccountId { get; set; }
        public required Guid TarifId { get; set; }
        public required int PeriodDays { get; set; }
        public required decimal LoanAmount { get; set; }
    }
}
