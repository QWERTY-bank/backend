using System.ComponentModel.DataAnnotations;

namespace Bank.Credits.Api.Models.Credits
{
    /// <summary>
    /// Запрос на уменьения суммы кредита
    /// </summary>
    public class ReduceCreditRequest
    {
        /// <summary>
        /// Уникальный идентификатор транзакции
        /// Для идемпотентности должен заполняться на стороне клиента
        /// </summary>
        [Required]
        public required Guid Key { get; init; }

        /// <summary>
        /// Номер счета, откуда списывать деньги
        /// </summary>
        [Required]
        public required long AccountId { get; set; }

        /// <summary>
        /// Сумма введенная для уменьшения долга по кредиту
        /// </summary>
        [Required]
        public required decimal Value { get; set; }
    }
}
