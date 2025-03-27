using Bank.Credits.Domain.Common.Constants;
using System.Diagnostics.CodeAnalysis;

namespace Bank.Credits.Domain.Credits
{
    public class CreditPaymentsInfo
    {
        /// <summary>
        /// На какой срок выдан кредит
        /// </summary>
        public required int PeriodDays { get; set; }

        /// <summary>
        /// Дата выдачи кредита, устанавливаем только, когда кредит переходит из "Ожидание" в статус "Активный" 
        /// </summary>
        public required DateOnly? TakingDate { get; set; }

        /// <summary>
        /// День последнего платежа по кредиту
        /// </summary>
        [NotNullIfNotNull(nameof(TakingDate))]
        public DateOnly? LastDate { get => TakingDate?.AddDays(PeriodDays - 1); }

        /// <summary>
        /// Сумма долга по кредиту в текущий момент 
        /// </summary>
        public required decimal DebtAmount { get; set; }

        /// <summary>
        /// Список остатков по кредиту с начисленными процентами перед платежом
        /// </summary>
        public Queue<decimal> DebtsWithInterest { get; set; } = null!;

        /// <summary>
        /// Последний раз, когда начислялся процент
        /// </summary>
        public DateOnly? LastInterestChargeDate { get; set; }

        /// <summary>
        /// Равный платеж по аннуитетному кредиту
        /// </summary>
        public decimal EqualPayment { get; set; }

        /// <summary>
        /// Дата следующего платежа
        /// </summary>
        [NotNullIfNotNull(nameof(TakingDate))]
        public DateOnly? NextPaymentDate { get; set; }

        /// <summary>
        /// Переносит дату следующего платежа на следующий период
        /// Если срок, на который взят кредит, не кратен числу дней в периоде, то последний платеж совершается в последний день срока, на который взят кредит, но
        /// это только в том случае, если текущий NextPaymentDate меньше LastDate, иначе следующий платеж будет в конце следующего периода (который идет в соответствии с TakingDate)
        /// </summary>
        public void UpdateNextPaymentDate()
        {
            var presumptiveNextPaymentDate = NextPaymentDate!.Value.AddDays(CreditConstants.PaymentPeriodDays);
            NextPaymentDate = presumptiveNextPaymentDate < LastDate
                ? presumptiveNextPaymentDate
                : NextPaymentDate < LastDate
                    ? LastDate
                    : presumptiveNextPaymentDate;
        }
    }
}
