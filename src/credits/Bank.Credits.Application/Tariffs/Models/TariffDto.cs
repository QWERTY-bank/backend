﻿using Bank.Credits.Domain.Tariffs;

namespace Bank.Credits.Application.Tariffs.Models
{
    public class TariffDto
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
