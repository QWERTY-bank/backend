using System.ComponentModel;

namespace Bank.Credits.Application.Credits
{
    public enum CreditStatusFilterType
    {
        [Description("Показать все кредиты")]
        All = 0,

        [Description("Показать только закрытые кредиты")]
        OnlyRepaid = 1,

        [Description("Показать только активные кредиты")]
        OnlyActive = 2,
    }
}
