using System.ComponentModel.DataAnnotations;

namespace Bank.Credits.Api.Models.Credits
{
    /// <summary>
    /// Запрос на уменьения суммы кредита
    /// </summary>
    public class ReduceCreditRequest
    {
        /// <summary>
        /// Сумма введенная для уменьшения долга по кредиту
        /// </summary>
        [Required]
        public required decimal Value { get; set; }
    }
}
