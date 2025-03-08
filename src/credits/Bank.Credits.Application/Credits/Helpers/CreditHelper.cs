using Bank.Credits.Application.Constants;
using Bank.Credits.Domain.Credits;
using Bank.Credits.Domain.Tariffs;

namespace Bank.Credits.Application.Credits.Helpers
{
    public static class CreditHelper
    {
        public static DateOnly CurrentDate { get => DateOnly.FromDateTime(DateTime.UtcNow); }

        public static DateOnly CalculateNextPaymentDate(Credit credit)
        {
            var passedDaysFromTaking = CurrentDate.DayNumber - credit.TakingDate!.Value.DayNumber;

            var existCurrentDateRepayment = credit.PaymentHistory?
                .Where(x => DateOnly.FromDateTime(x.PaymentDateTime) == CurrentDate)
                .Any(x => x.PaymentStatus == PaymentStatusType.Conducted && x.Type == PaymentType.Repayment) ?? false;

            var daysFromTakingToNext =
                CreditConstants.PaymentPeriodDays * (passedDaysFromTaking / CreditConstants.PaymentPeriodDays) +
                // Если текущая дата равна дате погашения, но было погашение на текущую дату, то берем следующую дату платежа
                passedDaysFromTaking % CreditConstants.PaymentPeriodDays == 0 && !existCurrentDateRepayment ? 0 : CreditConstants.PaymentPeriodDays;

            return credit.TakingDate.Value.AddDays(daysFromTakingToNext);
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

            var interestRateType = credit.CalculateInterestRateForPeriod();
            var temp = Pow(1 + interestRateType, remainedPaymentsCount);

            var paymentForPeriod = Math.Round(credit.DebtAmount * (interestRateType * temp) / (temp - 1), 2);

            credit.PaymentsInfo.Payment = (int)Math.Ceiling(paymentForPeriod);
            credit.PaymentsInfo.LastPayment = paymentForPeriod * remainedPaymentsCount - credit.PaymentsInfo.Payment * (remainedPaymentsCount - 1);
        }

        private static decimal CalculateInterestRateForPeriod(this Credit credit)
        {
            return CreditConstants.PaymentPeriodDays * (credit.Tariff!.InterestRateType switch
            {
                InterestRateType.Annual => credit.Tariff.InterestRate / 365,
                InterestRateType.Monthly => credit.Tariff.InterestRate / 30,
                InterestRateType.Daytime => credit.Tariff.InterestRate,
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
