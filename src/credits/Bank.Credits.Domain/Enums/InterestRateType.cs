using System.ComponentModel;

namespace Bank.Credits.Domain.Enums
{
    public enum InterestRateType
    {
        [Description("Годавая ставка")]
        Annual = 0,

        [Description("Ежемесечная ставка")]
        Monthly = 1,

        [Description("Дневная ставка")]
        Daytime = 2,
    }
}
