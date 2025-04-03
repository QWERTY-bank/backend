namespace Bank.Credits.Application.Requests.Models
{
    public class BalanceDto
    {
        public required Guid UserId { get; init; }
        public required bool IsClosed { get; init; }
        public required CurrencyValueDto CurrencyValue { get; init; }
    }
}
