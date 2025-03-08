using Bank.Credits.Domain.Common;

namespace Bank.Credits.Domain.Credits
{
    public class Payment : BaseEntity
    {
        public decimal PaymentAmount { get; set; }
        public DateTime PaymentDateTime { get; set; }
        public PaymentStatusType PaymentStatus { get; set; }
        public PaymentType Type { get; set; }

        public Guid CreditId { get; set; }
        public Credit? Credit { get; set; }
    }

    public class ReducePayment : Payment
    {
        ReducePayment()
        {
            Type = PaymentType.ReduceDebt;
        }
    }

    public class RepaymentPayment : Payment
    {
        RepaymentPayment()
        {
            Type = PaymentType.Repayment;
        }
    }
}
