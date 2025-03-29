using System.ComponentModel;

namespace Bank.Credits.Domain.Credits
{
    public enum PaymentStatusType
    {
        [Description("Обрабатывается")]
        InProcess = 0,

        [Description("Проведен")]
        Conducted = 1,

        [Description("Отменен")]
        Canceled = 2,

        [Description("Просрочен")]
        Overdue = 3,
    }
}
