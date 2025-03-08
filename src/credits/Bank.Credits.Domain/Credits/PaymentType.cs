using System.ComponentModel;

namespace Bank.Credits.Domain.Credits
{
    public enum PaymentType
    {
        [Description("Уменьшение платежей")]
        ReduceDebt = 0,

        [Description("Выплата за период")]
        Repayment = 1
    }
}
