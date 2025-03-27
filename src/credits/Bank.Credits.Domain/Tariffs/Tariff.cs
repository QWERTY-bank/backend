using Bank.Credits.Domain.Common;
using Bank.Credits.Domain.Common.Constants;

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

        public decimal InterestRateForPeriod
        {
            get => 1 + CreditConstants.PaymentPeriodDays * InterestRateType switch
            {
                InterestRateType.Annual => NormalizedInterestRate / 365,
                InterestRateType.Monthly => NormalizedInterestRate / 30,
                InterestRateType.Daytime => NormalizedInterestRate,
                _ => throw new NotImplementedException(),
            };
        }
    }
}
