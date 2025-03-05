namespace Bank.Credits.Application.Requests.Models
{
    public class CurrencyValueDto
    {
        public required CurrencyCode Code { get; init; }
        public required decimal Value { get; init; }
    }
}
