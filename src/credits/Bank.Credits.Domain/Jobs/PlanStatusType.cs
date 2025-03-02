using System.ComponentModel;

namespace Bank.Credits.Domain.Jobs
{
    public enum PlanStatusType
    {
        [Description("Ожидает обработки")]
        Wait = 0,

        [Description("В процессе обработки")]
        InProcess = 1,

        [Description("Готово")]
        Done = 2,

        [Description("Ошибка")]
        Error = 3
    }
}
