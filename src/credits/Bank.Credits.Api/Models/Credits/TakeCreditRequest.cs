using System.ComponentModel.DataAnnotations;

namespace Bank.Credits.Api.Models.Credits
{
    /// <summary>
    /// Запрос на получения кредита
    /// </summary>
    public class TakeCreditRequest : IValidatableObject
    {
        /// <summary>
        /// Уникальный идентификатор транзакции
        /// Для идемпотентности должен заполняться на стороне клиента
        /// </summary>
        [Required]
        public required Guid Key { get; init; }

        /// <summary>
        /// Номер счета, на который берем кредит
        /// </summary>
        public required long AccountId { get; set; }

        /// <summary>
        /// Id тарифа
        /// </summary>
        [Required]
        public required Guid TariffId { get; set; }

        /// <summary>
        /// На дней сколько берем кредит
        /// </summary>
        [Required]
        public required int PeriodDays { get; set; }

        /// <summary>
        /// Размер запрашиваемого кредита
        /// </summary>
        [Required]
        public required decimal LoanAmount { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (PeriodDays <= 0)
            {
                yield return new ValidationResult("PeriodDays must be greater than 0", new[] { nameof(PeriodDays) });
            }

            if (LoanAmount <= 0)
            {
                yield return new ValidationResult("LoanAmount must be greater than 0", new[] { nameof(LoanAmount) });
            }
        }
    }
}
