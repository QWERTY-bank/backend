using Bank.Credits.Application.Enums;

namespace Bank.Credits.Api.Models.Credits
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
