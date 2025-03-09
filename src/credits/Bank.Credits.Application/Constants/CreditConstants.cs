namespace Bank.Credits.Application.Constants
{
    public static class CreditConstants
    {
        /// <summary>
        /// Срок, в последний день которого нужно внести платеж по кредиту
        /// </summary>
        public static readonly int PaymentPeriodDays = 1;

        public static readonly TimeSpan DayLength = new TimeSpan(hours: 0, minutes: 10, seconds: 0);
    }
}
