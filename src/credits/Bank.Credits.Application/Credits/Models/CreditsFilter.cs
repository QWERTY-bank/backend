namespace Bank.Credits.Application.Credits.Models
{
    /// <summary>
    /// Фильтры для истории кредитов
    /// </summary>
    public class CreditsFilter
    {
        /// <summary>
        /// Фильтр по статусу кредита
        /// </summary>
        public CreditStatusFilterType CreditStatus { get; set; } = CreditStatusFilterType.All;
    }
}
