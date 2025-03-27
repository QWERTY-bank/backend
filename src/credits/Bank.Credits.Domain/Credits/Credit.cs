using Bank.Credits.Domain.Common;
using Bank.Credits.Domain.Common.Helpers;
using Bank.Credits.Domain.Tariffs;

namespace Bank.Credits.Domain.Credits
{
    public class Credit : JobPlannedBaseEntity
    {
        public CreditPaymentsInfo PaymentsInfo { get; set; } = null!;

        /// <summary>
        /// Ключ идемпотентности, с которым был создан кредит
        /// </summary>
        public required Guid Key { get; init; }

        /// <summary>
        /// Id пользователя, к которому привязан кредит
        /// </summary>
        public required Guid UserId { get; set; }

        /// <summary>
        /// Номер счета, на который выдан кредит
        /// </summary>
        public required long AccountId { get; set; }

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
        public List<Payment>? PaymentHistory { get; set; }

        /// <summary>
        /// Переводит активный кредит в статус Closed, если сумма долга составляет меньше нуля
        /// </summary>
        private void UpdateCreditStatus()
        {
            if (Status == CreditStatusType.Active && PaymentsInfo.DebtAmount <= 0.0M)
            {
                Status = CreditStatusType.Closed;
            }
        }

        /// <summary>
        /// Начисляет процент по кредиту за текущий период
        /// Если сейчас день платежа и процент уже был начислен, то ничего не произойдет (Нужно в тех случаях, 
        /// когда планер создал платеж, который в последствии отменился из-за ошибки, тогда планер еще раз 
        /// создаст платеж, а так как процент начисляется в хендлере, то произойдет повторный вызов начисления 
        /// процента, который начислять уже не нужно)
        /// Если сейчас не день платежа, то вернет false
        /// </summary>
        public bool ApplyInterestRate()
        {
            if (PaymentsInfo.NextPaymentDate != DateHelper.CurrentDate)
            {
                return false;
            }

            if (PaymentsInfo.LastInterestChargeDate == DateHelper.CurrentDate)
            {
                PaymentsInfo.DebtAmount = PaymentsInfo.DebtsWithInterest.Peek();
                PaymentsInfo.LastInterestChargeDate = DateHelper.CurrentDate;
            }

            return true;
        }

        /// <summary>
        /// Совершаем платеж согласно информации о платежах по кредиту
        /// Вычитает из суммы долга равный платеж и ставит следующую дату платежа, а также обновляет статус кредита
        /// Вернет false если кредит не активен
        /// Вернет false, если процент по кредиту не начислен перед платежом или сейчас не день платежа
        /// Если обнаружилась ошибка в расчетах, то вернет false
        /// </summary>
        public bool MakePayment()
        {
            if (Status != CreditStatusType.Active)
            {
                return false;
            }

            if (PaymentsInfo.LastInterestChargeDate != DateHelper.CurrentDate || PaymentsInfo.NextPaymentDate != DateHelper.CurrentDate)
            {
                return false;
            }

            if (PaymentsInfo.EqualPayment < PaymentsInfo.DebtAmount)
            {
                return false;
            }

            PaymentsInfo.DebtAmount -= PaymentsInfo.EqualPayment;
            PaymentsInfo.UpdateNextPaymentDate();

            UpdateCreditStatus();

            return true;
        }

        /// <summary>
        /// Уменьшает платеж на указанную сумму и вызывает UpdatePaymentsInfo
        /// Если указанная сумма больше суммы долга, то вернет false, инчае если уменьшение прошло успешно, то true
        /// </summary>
        public bool ReduceDebt(decimal value)
        {
            if (value < PaymentsInfo.DebtAmount)
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

            // TODO: расчет платежей

            // Вычислить равный платеж (последний платеж равен последнему остатку)
            // Вычислить суммы остатков с процентами

            // Если остался один период то сразу записываем в платеж сумму долга а в массив остатков с процентами один остаток с процентом

        }

        /// <summary>
        /// Вычисляет аннуитетный коэффициент
        /// </summary>
        /// <param name="interestRate">Процентная ставка</param>
        /// <param name="periodDays"></param>
        /// <returns></returns>
        private static decimal CalculateAnnualCoeff(decimal interestRate, int periodDays)
        {
            var temp = MathHelper.Pow(1 + interestRate, periodDays);
            return interestRate * temp / (temp - 1);
        }
    }
}

/*
 
        /// <summary>
        /// X - равный платеж, кроме последнего Y, который либо равен либо меньше X
        /// D - сколько должны сейчас
        /// y - сколько платежей = На сколько дней взяли \ продолжительность периода в днях - (Дата взятия - Текущая дата)
        /// i процент за день/месяц/год
        /// 
        /// Формула аннуитетного платежа
        /// X = cell(D * (i*(1+i)^y)/((1 + i)^y - 1))
        /// Y = D - X * y
        /// 
        /// Нужно загрузить PaymentHistory и Tariff
        /// </summary>
        public static void UpdateCreditPaymentsInfo(this Credit credit)
        {
            // Сохраняем в кредит X и Y, при уменьшении кредита в ReduceCreditAsync пересчитываем их
            // 
            // При авто-погашении сначала умножаем текущий долг на процент потом вычитаем от туда X или Y если это последний платеж и сохраняем в сумму долга
            // 
            // Если предыдущего погашения не было -> пересчитываем X и Y для суммы долга с учетом процента за пропущенные периоды
            // .

            var nextPaymentDate = credit.CalculateNextPaymentDate();

            var daysLeft = credit.LastDate!.Value.DayNumber - nextPaymentDate.DayNumber + 1;
            var remainedPaymentsCount = daysLeft > 0
                ? daysLeft / CreditConstants.PaymentPeriodDays + (daysLeft % CreditConstants.PaymentPeriodDays == 0 ? 0 : 1)
                : 1;

            var interestRate = credit.CalculateInterestRateForPeriod();

            var payment = credit.DebtAmount * CalculateAnnualCoeff(interestRate, remainedPaymentsCount);

            // Если начислили процент, но автоперевод еще не прошел, и в это промежуток пользователь уменьшил платеж, то не учитываем процент за этот период
            if (credit.LastInterestChargeDate == DateHelper.CurrentDate 
                && !credit.PaymentHistory!.Any(x => x.Type == PaymentType.Repayment && x.PaymentDate == DateHelper.CurrentDate && x.PaymentStatus == PaymentStatusType.Conducted))
            {
                payment = remainedPaymentsCount == 1
                    ? credit.DebtAmount
                    : credit.DebtAmount * CalculateAnnualCoeff(interestRate, remainedPaymentsCount - 1) / (CalculateAnnualCoeff(interestRate, remainedPaymentsCount - 1) + 1);
            }

            credit.PaymentsInfo.Payment = (int)Math.Ceiling(payment);
            credit.PaymentsInfo.LastPayment = payment * remainedPaymentsCount - credit.PaymentsInfo.Payment * (remainedPaymentsCount - 1);
        }


 
 */