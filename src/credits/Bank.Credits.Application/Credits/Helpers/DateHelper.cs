using Bank.Credits.Application.Constants;

namespace Bank.Credits.Application.Credits.Helpers
{
    public static class DateHelper
    {
        public static DateOnly CurrentDate
        {
            get
            {
                var startFrom = new DateTime(2025, 03, 09);
                var now = DateTime.UtcNow;

                var passedSeconds = (now - startFrom);

                var passedDays = (int)(passedSeconds.Ticks / CreditConstants.DayLength.Ticks);

                return DateOnly.FromDateTime(startFrom).AddDays(passedDays);
            }
        }
    }
}
