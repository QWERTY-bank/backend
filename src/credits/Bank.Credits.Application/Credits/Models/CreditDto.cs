using Bank.Credits.Domain.Credits;

namespace Bank.Credits.Application.Credits.Models
{
    public class CreditDto
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
        /// Дата взятия кредита
        /// </summary>
        public required DateOnly? TakingDate { get; init; }

        /// <summary>
        /// Следующие платежи
        /// </summary>
        public required List<NextPaymentDto> NextPayments { get; init; }

        /// <summary>
        /// История платежей
        /// </summary>
        public required List<PaymentDto> PaymentHistory { get; init; }
    }
}