using Bank.Credits.Domain.Common;
using Bank.Credits.Domain.Common.Constants;
using Bank.Credits.Domain.Common.Helpers;
using Bank.Credits.Domain.Tariffs;
using Bank.Credits.Domain.User;

namespace Bank.Credits.Domain.Credits
{
    public class Credit : JobPlannedBaseEntity
    {
        public CreditPaymentsInfo PaymentsInfo { get; set; } = new();

        /// <summary>
        /// Id пользователя, к которому привязан кредит
        /// </summary>
        public Guid UserId { get; set; }

        public UserEntity? User { get; set; }

        /// <summary>
        /// Номер счета, на который выдан кредит
        /// </summary>
        public required long AccountId { get; set; }

        /// <summary>
        /// Валюта счета, на который выдан кредит
        /// </summary>
        public required CurrencyCode CurrencyCode { get; set; }

        /// <summary>
        /// Статус кредита
        /// </summary>
        public required CreditStatusType Status { get; set; }

        /// <summary>
        /// Тариф, по которому выдан кредит
        /// </summary>
        public required Guid TariffId { get; set; }
        public Tariff? Tariff { get; set; }

        /// <summary>
        /// История платежей по кредиту
        /// </summary>
        public List<Payment> PaymentHistory { get; set; } = null!;

        public void CancelCredit()
        {
            Status = CreditStatusType.Canceled;
        }

        public void TakeCredit()
        {
            Status = CreditStatusType.Active;
            PaymentsInfo.TakingDate = DateHelper.CurrentDate;
            PaymentsInfo.UpdateNextPaymentDate();
            UpdatePaymentsInfo();
        }

        /// <summary>
        /// Переводит активный кредит в статус Closed, если сумма долга составляет меньше нуля
        /// </summary>
        public void UpdateCreditStatus()
        {
            if (Status == CreditStatusType.Active && PaymentsInfo.DebtAmount <= 0.0M)
            {
                Status = CreditStatusType.Closed;
            }
            else if (PaymentsInfo.DebtAmount > 0.0M)
            {
                Status = CreditStatusType.Active;
            }
        }

        /// <summary>
        /// Начисляет процент по кредиту за текущий период
        /// Если сейчас день платежа и процент уже был начислен, то ничего не произойдет (Нужно в тех случаях, 
        /// когда планер создал платеж, который в последствии отменился из-за ошибки, тогда планер еще раз 
        /// создаст платеж, а так как процент начисляется в хендлере, то произойдет повторный вызов начисления 
        /// процента, который начислять уже не нужно)
        /// Если сейчас не день платежа, то вернет false
        /// Если рассчитанные платежи закончились, то умножает текущий долг на процентную ставку
        /// </summary>
        public bool ApplyInterestRate()
        {
            if (PaymentsInfo.NextPaymentDate != DateHelper.CurrentDate)
            {
                return false;
            }

            if (PaymentsInfo.LastInterestChargeDate == DateHelper.CurrentDate)
            {
                return true;
            }

            if (PaymentsInfo.DebtsWithInterest.Any())
            {
                PaymentsInfo.DebtAmount = PaymentsInfo.DebtsWithInterest.Dequeue();
                PaymentsInfo.DebtsWithInterest = new(PaymentsInfo.DebtsWithInterest); // Обновляем очередь
            }
            else
            {
                PaymentsInfo.DebtAmount = MathHelper.Multiplies(PaymentsInfo.DebtAmount, Tariff!.IncrementallyInterestRateForPeriod);
            }

            PaymentsInfo.LastInterestChargeDate = DateHelper.CurrentDate;

            return true;
        }

        /// <summary>
        /// Совершаем платеж согласно информации о платежах по кредиту
        /// Вычитает из суммы долга равный платеж и ставит следующую дату платежа, а также обновляет статус кредита
        /// Вернет false если кредит не активен
        /// Вернет false, если процент по кредиту не начислен перед платежом или сейчас не день платежа
        /// Если обнаружилась ошибка в расчетах, то вернет false
        /// </summary>
        public bool MakeRepayment()
        {
            if (Status != CreditStatusType.Active)
            {
                return false;
            }

            if (PaymentsInfo.LastInterestChargeDate != DateHelper.CurrentDate || PaymentsInfo.NextPaymentDate != DateHelper.CurrentDate)
            {
                return false;
            }

            PaymentsInfo.DebtAmount -= PaymentsInfo.NextPayment;
            PaymentsInfo.UpdateNextPaymentDate();

            return true;
        }

        /// <summary>
        /// Возвращает деньги в долг и обновляет информацию о платежах
        /// </summary>
        public bool CancelPayment(decimal value)
        {
            PaymentsInfo.DebtAmount += value;
            
            UpdatePaymentsInfo();

            return true;
        }

        /// <summary>
        /// Уменьшает платеж на указанную сумму и вызывает UpdatePaymentsInfo
        /// Если указанная сумма больше суммы долга, то вернет false, иначе если уменьшение прошло успешно, то true
        /// </summary>
        public bool ReduceDebt(decimal value)
        {
            if (PaymentsInfo.DebtAmount < value)
            {
                return false;
            }

            PaymentsInfo.DebtAmount -= value;
            UpdatePaymentsInfo();

            return true;
        }

        /// <summary>
        /// Обновляем информацию о платежах согласно текущему долгу, информации о прострочках и т.д., но
        /// перед этим обновляет статус по кредиту, если долго выплачен, то переводит кредит в статус Closed
        /// Если кредит имеет НЕ статус Active, то ничего не произойдет
        /// </summary>
        public void UpdatePaymentsInfo()
        {
            UpdateCreditStatus();

            if (Status != CreditStatusType.Active)
            {
                return;
            }

            //if (PaymentsInfo.LastDate < PaymentsInfo.NextPaymentDate)
            //{
            //    return;
            //}

            var remainedPaymentsCount = PaymentsInfo.RemainedPaymentsCount;

            //////(неактуально)Если процент начислялся и платеж на погашение еще не прошел или он отменился, но клиент уменьшил сумму кредита, то
            //////платеж по кредиту должен быть пересчитан с учетом того, что процент за период уже начислен
            // 
            //
            if (PaymentsInfo.LastInterestChargeDate == DateHelper.CurrentDate && PaymentsInfo.NextPaymentDate == DateHelper.CurrentDate) // && !PaymentHistory!.Any(x => x.Type == PaymentType.Repayment && x.PaymentDate == DateHelper.CurrentDate && x.PaymentStatus == PaymentStatusType.Conducted)
            {
                if (remainedPaymentsCount == 1)
                {
                    return;
                }

                var annualCoeff = CalculateAnnualCoeff(Tariff!.InterestRateForPeriod, remainedPaymentsCount - 1);
                PaymentsInfo.EqualPayment = MathHelper.Multiplies(PaymentsInfo.DebtAmount, annualCoeff / (annualCoeff + 1));

                PaymentsInfo.DebtsWithInterest = CalculateDebtsWithInterest(
                    initDebt: PaymentsInfo.DebtAmount - PaymentsInfo.EqualPayment,
                    equalPayment: PaymentsInfo.EqualPayment,
                    remainedPaymentsCount: remainedPaymentsCount - 1);
            }
            // В другом случае считаем по обычной формуле
            else
            {
                var annualCoeff = CalculateAnnualCoeff(Tariff!.InterestRateForPeriod, remainedPaymentsCount);
                PaymentsInfo.EqualPayment = MathHelper.Multiplies(PaymentsInfo.DebtAmount, annualCoeff);

                PaymentsInfo.DebtsWithInterest = CalculateDebtsWithInterest(
                    initDebt: PaymentsInfo.DebtAmount,
                    equalPayment: PaymentsInfo.EqualPayment,
                    remainedPaymentsCount);
            }
        }

        private Queue<decimal> CalculateDebtsWithInterest(decimal initDebt, decimal equalPayment, int remainedPaymentsCount)
        {
            var debtsWithInterestRate = new List<decimal>();

            var debt = initDebt;
            for (int i = 0; i < remainedPaymentsCount; i++)
            {
                debt = MathHelper.Multiplies(debt, Tariff!.IncrementallyInterestRateForPeriod);

                debtsWithInterestRate.Add(debt);

                debt -= equalPayment;
            }

            return new(debtsWithInterestRate);
        }

        /// <summary>
        /// Вычисляет аннуитетный коэффициент
        /// </summary>
        /// <param name="interestRate">Процентная ставка</param>
        /// <param name="periodDays"></param>
        /// <returns></returns>
        private static decimal CalculateAnnualCoeff(decimal interestRate, int periodsCount)
        {
            var temp = MathHelper.Pow(1 + interestRate, periodsCount);
            return interestRate * temp / (temp - 1);
        }

        public IEnumerable<Payment> NextRepayments()
        {
            if (Status != CreditStatusType.Active || PaymentsInfo.DebtAmount == 0.0M)
            {
                yield break;
            }

            var nextDate = PaymentsInfo.NextPaymentDate;

            while (nextDate < PaymentsInfo.LastDate)
            {
                yield return new RepaymentPayment
                {
                    Key = Guid.NewGuid(),
                    PaymentAmount = PaymentsInfo.EqualPayment,
                    PaymentStatus = PaymentStatusType.InProcess,
                    PaymentDate = nextDate.Value,
                    CreditId = Id
                };

                nextDate = nextDate!.Value.AddDays(CreditConstants.PaymentPeriodDays);
            }

            if (nextDate >= PaymentsInfo.LastDate)
            {
                yield return new RepaymentPayment
                {
                    Key = Guid.NewGuid(),
                    PaymentAmount = PaymentsInfo.DebtsWithInterest.LastOrDefault(PaymentsInfo.DebtAmount),
                    PaymentStatus = PaymentStatusType.InProcess,
                    PaymentDate = PaymentsInfo.NextPaymentDate < PaymentsInfo.LastDate
                        ? PaymentsInfo.LastDate!.Value : PaymentsInfo.NextPaymentDate!.Value,
                    CreditId = Id
                };
            }
        }
    }
}
