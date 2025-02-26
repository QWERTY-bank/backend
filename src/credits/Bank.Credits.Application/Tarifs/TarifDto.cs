using Bank.Credits.Domain.Enums;

namespace Bank.Credits.Application.Tarifs
{
    public class TarifDto
    {
        public required Guid Id { get; init; }

        /// <summary>
        /// Название тарифа
        /// </summary>
        public required string Name { get; init; }

        /// <summary>
        /// Процентная ставка
        /// </summary>
        public required decimal InterestRate { get; init; }

        /// <summary>
        /// Тип процентной ставки
        /// </summary>
        public required InterestRateType InterestRateType { get; init; }

        /// <summary>
        /// Минимальное срок, на который берется кредит
        /// </summary>
        public required int MinPeriodDays { get; init; }

        /// <summary>
        /// Максимальный срок, на который берется кредит
        /// </summary>
        public required int MaxPeriodDays { get; init; }
    }
}
