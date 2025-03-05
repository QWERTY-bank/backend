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
    }
}
