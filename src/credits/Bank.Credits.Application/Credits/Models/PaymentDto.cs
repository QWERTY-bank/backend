using Bank.Credits.Domain.Credits;

namespace Bank.Credits.Application.Credits.Models
{
    public class PaymentDto
    {
        /// <summary>
        /// Сумма платежа
        /// </summary>
        public required decimal PaymentAmount { get; init; }

        /// <summary>
        /// Дата и время совершенного платежа
        /// </summary>
        public required DateTime PaymentDateTime { get; init; }

        /// <summary>
        /// Статус платежа
        /// </summary>
        public required PaymentStatusType PaymentStatus { get; init; }

        /// <summary>
        /// Тип платежа
        /// </summary>
        public required PaymentType PaymentType { get; init; }
    }
}
