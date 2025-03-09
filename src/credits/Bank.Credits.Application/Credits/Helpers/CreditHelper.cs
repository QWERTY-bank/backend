using Bank.Credits.Application.Constants;
using Bank.Credits.Domain.Credits;
using Bank.Credits.Domain.Tariffs;

namespace Bank.Credits.Application.Credits.Helpers
{
    public static class CreditHelper
    {
        /// <summary>
        /// Нужно загрузить PaymentHistory
        /// </summary>
        public static DateOnly CalculateNextPaymentDate(this Credit credit)
        {
            var passedDaysFromTaking = DateHelper.CurrentDate.DayNumber - credit.TakingDate!.Value.DayNumber;

            var existCurrentDateRepayment = credit.PaymentHistory?
                .Where(x => x.PaymentDate == DateHelper.CurrentDate)
                .Any(x => x.PaymentStatus == PaymentStatusType.Conducted && x.Type == PaymentType.Repayment) ?? false;

            var daysFromTakingToNext =
                CreditConstants.PaymentPeriodDays * (passedDaysFromTaking / CreditConstants.PaymentPeriodDays) +
                // Если текущая дата равна дате погашения, но было погашение на текущую дату, то берем следующую дату платежа
                (passedDaysFromTaking % CreditConstants.PaymentPeriodDays == 0 && !existCurrentDateRepayment ? 0 : CreditConstants.PaymentPeriodDays);

            return credit.TakingDate.Value.AddDays(daysFromTakingToNext);
        }

        public static decimal CalculateNextPaymentAmount(this Credit credit)
        {
            var nextDate = CalculateNextPaymentDate(credit);

            return nextDate < credit.LastDate
                ? credit.PaymentsInfo.Payment
                : credit.PaymentsInfo.LastPayment;
        }

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

            var nextPaymentDate = CalculateNextPaymentDate(credit);

            var daysLeft = credit.LastDate!.Value.DayNumber - nextPaymentDate.DayNumber + 1;
            var remainedPaymentsCount = daysLeft > 0
                ? (daysLeft / CreditConstants.PaymentPeriodDays) + (daysLeft % CreditConstants.PaymentPeriodDays == 0 ? 0 : 1)
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

        private static decimal CalculateAnnualCoeff(decimal interestRateType, int remainedPaymentsCount)
        {
            var temp = Pow(1 + interestRateType, remainedPaymentsCount);
            return (interestRateType * temp) / (temp - 1);
        }

        public static void ApplyInterestRate(this Credit credit)
        {
            credit.DebtAmount *= (1 + credit.CalculateInterestRateForPeriod());
            credit.LastInterestChargeDate = DateHelper.CurrentDate;
        }

        public static void MakePayment(this Credit credit, decimal value)
        {
            credit.DebtAmount -= value;

            if (credit.DebtAmount <= 0)
            {
                credit.Status = CreditStatusType.Closed;
                credit.DebtAmount = 0;
            }
        }

        public static void ReduceDebt(this Credit credit, decimal value)
        {
            credit.MakePayment(value);
            credit.UpdateCreditPaymentsInfo();
        }

        private static decimal CalculateInterestRateForPeriod(this Credit credit)
        {
            return CreditConstants.PaymentPeriodDays * (credit.Tariff!.InterestRateType switch
            {
                InterestRateType.Annual => credit.Tariff.NormalizedInterestRate / 365,
                InterestRateType.Monthly => credit.Tariff.NormalizedInterestRate / 30,
                InterestRateType.Daytime => credit.Tariff.NormalizedInterestRate,
                _ => throw new NotImplementedException(),
            });
        }

        private static decimal Pow(decimal baseNum, int exponent)
        {
            if (exponent == 0) return 1;
            if (exponent < 0) return 1 / Pow(baseNum, -exponent);

            decimal result = 1;
            for (int i = 0; i < exponent; i++)
            {
                result *= baseNum;
            }
            return result;
        }
    }
}
