namespace Bank.Credits.Application.Requests.Models
{
    public class TransactionDto
    {
        public required Guid Key { get; init; }
        public required List<CurrencyValueDto> CurrencyValues { get; init; }
    }
}
