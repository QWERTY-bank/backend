using Bank.Credits.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Bank.Credits.Api.Models.Tarifs
{
    /// <summary>
    /// Запрос на создание запроса
    /// </summary>
    public class CreateTariffRequest
    {
        /// <summary>
        /// Название тарифа
        /// </summary>
        [Required]
        public required string Name { get; set; }

        /// <summary>
        /// Процентная ставка
        /// </summary>
        [Required]
        public required decimal InterestRate { get; set; }

        /// <summary>
        /// Тип процентной ставки
        /// </summary>
        [Required]
        public required InterestRateType InterestRateType { get; set; }

        /// <summary>
        /// Минимальное срок, на который берется кредит
        /// </summary>
        [Required]
        public required int MinPeriodDays { get; set; }

        /// <summary>
        /// Максимальный срок, на который берется кредит
        /// </summary>
        [Required]
        public required int MaxPeriodDays { get; set; }
    }
}
