using System.ComponentModel.DataAnnotations;

namespace Bank.Credits.Api.Models.Credits
{
    public class ReduceCreditRequest
    {
        /// <summary>
        /// Сумма введенная для уменьшения долга по кредиту
        /// </summary>
        [Required]
        public required decimal Value { get; set; }
    }
}
