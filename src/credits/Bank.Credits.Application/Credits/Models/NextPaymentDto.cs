namespace Bank.Credits.Application.Credits.Models
{
    public class NextPaymentDto
    {
        /// <summary>
        /// Сумма платежа
        /// </summary>
        public required decimal PaymentAmount { get; init; }

        /// <summary>
        /// Дата платежа
        /// </summary>
        public required DateOnly PaymentDateOnly { get; init; }
    }
}