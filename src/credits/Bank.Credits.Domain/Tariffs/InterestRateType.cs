using System.ComponentModel;

namespace Bank.Credits.Domain.Tariffs
{
    public enum InterestRateType
    {
        [Description("Годовая ставка")]
        Annual = 0,

        [Description("Ежемесячная ставка")]
        Monthly = 1,

        [Description("Дневная ставка")]
        Daytime = 2,
    }
}
