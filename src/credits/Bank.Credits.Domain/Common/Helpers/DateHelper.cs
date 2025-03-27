using Bank.Credits.Domain.Common.Constants;

namespace Bank.Credits.Domain.Common.Helpers
{
    public static class DateHelper
    {
        public static DateOnly CurrentDate
        {
            get
            {
                //return DateOnly.FromDateTime(DateTime.Now);
                var startFrom = new DateTime(2025, 03, 27);
                var now = DateTime.UtcNow;

                var passedSeconds = now - startFrom;

                var passedDays = (int)(passedSeconds.Ticks / CreditConstants.DayLength.Ticks);

                return DateOnly.FromDateTime(startFrom).AddDays(passedDays);
            }
        }
    }
}
