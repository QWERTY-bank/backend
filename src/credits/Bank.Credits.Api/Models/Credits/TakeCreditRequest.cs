using System.ComponentModel.DataAnnotations;

namespace Bank.Credits.Api.Models.Credits
{
    /// <summary>
    /// Запрос на получения кредита
    /// </summary>
    public class TakeCreditRequest
    {
        /// <summary>
        /// Уникальный идентификатор транзакции
        /// Для идемпотентности должен заполняться на стороне клиента
        /// </summary>
        [Required]
        public required Guid Key { get; init; }

        /// <summary>
        /// Id тарифа
        /// </summary>
        [Required]
        public required Guid TarifId { get; set; }

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
    }
}
