using Bank.Credits.Domain.Common;

namespace Bank.Credits.Domain.Tariffs
{
    public class Tariff : SoftDeleteBaseEntity
    {
        public required string Name { get; set; }
        public required decimal InterestRate { get; set; }
        public decimal NormalizedInterestRate { get => InterestRate / 100.0M; }
        public required InterestRateType InterestRateType { get; set; }
        public required int MinPeriodDays { get; set; }
        public required int MaxPeriodDays { get; set; }
    }
}
