using Bank.Credits.Domain.Credits;

namespace Bank.Credits.Application.Credits.Models
{
    public class CreditShortDto
    {
        public required Guid Id { get; init; }

        /// <summary>
        /// Статус кредита
        /// </summary>
        public required CreditStatusType Status { get; set; }

        /// <summary>
        /// Сумма долга
        /// </summary>
        public required decimal DebtAmount { get; init; }

        /// <summary>
        /// Сумма следующего платежа
        /// </summary>
        public required decimal NextPaymentAmount { get; set; }

        /// <summary>
        /// Дата следующего платежа
        /// </summary>
        public required DateOnly NextPaymentDateOnly { get; set; }
    }
}
