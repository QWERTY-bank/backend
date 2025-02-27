using System.ComponentModel.DataAnnotations;

namespace Bank.Credits.Api.Models.Credits
{
    /// <summary>
    /// Запрос на получения кредита
    /// </summary>
    public class TakeCreditRequest
    {
        /// <summary>
        /// Id тарифа
        /// </summary>
        public required Guid TarifId { get; set; }

        /// <summary>
        /// На дней сколько берем кредит
        /// </summary>
        [Required]
        public required int PeriodDays { get; set; }
    }
}
