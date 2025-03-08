namespace Bank.Credits.Domain.Credits
{
    public class CreditPaymentsInfo
    {
        /// <summary>
        /// Размер платежа по всем периодам кроме последнего
        /// </summary>
        public int Payment { get; set; }

        /// <summary>
        /// Размер платежа по последнему периоду
        /// </summary>
        public decimal LastPayment { get; set; }
    }
}
