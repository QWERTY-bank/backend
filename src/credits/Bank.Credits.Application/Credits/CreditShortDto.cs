namespace Bank.Credits.Application.Credits
{
    public class CreditShortDto
    {
        public required Guid Id { get; init; }

        /// <summary>
        /// Сумма долга
        /// </summary>
        public required decimal DebtAmount { get; init; }

        /// <summary>
        /// Сумма следующего платежа
        /// </summary>
        public required decimal NextPaymentAmount { get; init; }

        /// <summary>
        /// Дата следующего платежа
        /// </summary>
        public required DateOnly NextPaymentDateOnly { get; init; }

        /// <summary>
        /// Погашен ли кредит
        /// </summary>
        public required bool IsRepaid { get; init; }
    }
}
