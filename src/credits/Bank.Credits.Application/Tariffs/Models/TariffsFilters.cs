using Bank.Credits.Domain.Tariffs;

namespace Bank.Credits.Application.Tariffs.Models
{
    public class TariffsFilters
    {
        /// <summary>
        /// Имя тарифа
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Максимальная сумма кредита
        /// </summary>
        public decimal? MaxInterestRate  { get; set; }

        /// <summary>
        /// За какой период начисляется процент
        /// </summary>
        public InterestRateType? InterestRateType { get; set; }

        /// <summary>
        /// На сколько необходимо взять кредит
        /// </summary>
        public int? PeriodDays { get; set; }
    }
}
