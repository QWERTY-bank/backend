using Bank.Credits.Domain.Common;
using Bank.Credits.Domain.Tariffs;

namespace Bank.Credits.Domain.Credits
{
    public class Credit : JobPlannedBaseEntity
    {
        /// <summary>
        /// Ключ идемпотентности, с которым был создан кредит
        /// </summary>
        public required Guid Key { get; init; }

        /// <summary>
        /// Id пользователя, к которому привязан кредит
        /// </summary>
        public required Guid UserId { get; set; }

        /// <summary>
        /// Статус кредита
        /// </summary>
        public required CreditStatusType Status { get; set; }

        /// <summary>
        /// На какой срок выдан кредит
        /// </summary>
        public required int PeriodDays { get; set; }

        /// <summary>
        /// Дата выдачи кредита, устанавливаем только, когда кредит переходит из "Ожидание" в статус "Активный" 
        /// </summary>
        public required DateOnly? TakingDate { get; set; }

        /// <summary>
        /// Сумма долга по кредиту в текущий момент 
        /// </summary>
        public required decimal DebtAmount { get; set; }

        /// <summary>
        /// Id тарифа, по которому выдан кредит
        /// </summary>
        public required Guid TariffId { get; set; }

        /// <summary>
        /// Тариф, по которому выдан кредит
        /// </summary>
        public Tariff? Tariff { get; set; }
    }
}
