using Bank.Credits.Domain.Tariffs;
using System.ComponentModel.DataAnnotations;

namespace Bank.Credits.Api.Models.Tariffs
{
    /// <summary>
    /// Запрос на создание запроса
    /// </summary>
    public class CreateTariffRequest : IValidatableObject
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

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (InterestRate <= 0)
            {
                yield return new ValidationResult("InterestRate must be greater than 0", new[] { nameof(InterestRate) });
            }

            if (MinPeriodDays <= 0)
            {
                yield return new ValidationResult("MinPeriodDays must be greater than 0", new[] { nameof(MinPeriodDays) });
            }

            if (MinPeriodDays > MaxPeriodDays)
            {
                yield return new ValidationResult("MinPeriodDays must be less than or equal to MaxPeriodDays", new[] { nameof(MinPeriodDays), nameof(MaxPeriodDays) });
            }
        }
    }
}
