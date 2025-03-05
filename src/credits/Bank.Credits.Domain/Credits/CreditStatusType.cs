using System.ComponentModel;

namespace Bank.Credits.Domain.Credits
{
    public enum CreditStatusType
    {
        [Description("Запрошен")]
        Requested = 0,

        [Description("Активный")]
        Active = 1,

        [Description("Закрыт")]
        Closed = 2,

        [Description("Отменен")]
        Canceled = 3
    }
}
