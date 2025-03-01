using Bank.Credits.Domain.Tariffs;

namespace Bank.Credits.Application.Tariffs.Models
{
    public class CreateTariffDto
    {
        public required string Name { get; set; }
        public required decimal InterestRate { get; set; }
        public required InterestRateType InterestRateType { get; set; }
        public required int MinPeriodDays { get; set; }
        public required int MaxPeriodDays { get; set; }
    }
}
