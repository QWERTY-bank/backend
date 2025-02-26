using Bank.Credits.Application.Payments;

namespace Bank.Credits.Application.Credits
{
    public class CreditDto
    {
        public required Guid Id { get; init; }

        /// <summary>
        /// Сумма долга
        /// </summary>
        public required decimal DebtAmount { get; init; }

        /// <summary>
        /// Погашен ли кредит
        /// </summary>
        public required bool IsRepaid { get; init; }

        /// <summary>
        /// Дата взятия кредита
        /// </summary>
        public required DateOnly TakingDate { get; init; }

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