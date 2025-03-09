using Bank.Credits.Domain.Common;

namespace Bank.Credits.Domain.Credits
{
    public class Payment : JobPlannedBaseEntity
    {
        /// <summary>
        /// Ключ идемпотентности, с которым был создан платеж
        /// </summary>
        public required Guid Key { get; init; }

        /// <summary>
        /// Номер счета, с которого были списаны деньги
        /// </summary>
        public required long AccountId { get; set; }

        public required decimal PaymentAmount { get; set; }
        public required DateTime PaymentDateTime { get; set; }
        public required PaymentStatusType PaymentStatus { get; set; }
        public PaymentType Type { get; set; }

        public required Guid CreditId { get; set; }
        public Credit? Credit { get; set; }
    }

    public class ReducePayment : Payment
    {
        public ReducePayment()
        {
            Type = PaymentType.ReduceDebt;
        }
    }

    public class RepaymentPayment : Payment
    {
        public RepaymentPayment()
        {
            Type = PaymentType.Repayment;
        }
    }
}
