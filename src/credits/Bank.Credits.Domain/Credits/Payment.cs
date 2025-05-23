﻿using Bank.Credits.Domain.Common;
using System.Diagnostics.CodeAnalysis;

namespace Bank.Credits.Domain.Credits
{
    public class Payment : JobPlannedBaseEntity
    {
        /// <summary>
        /// Ключ идемпотентности, с которым был создан платеж
        /// </summary>
        public Guid Key { get; init; }

        /// <summary>
        /// Номер счета, с которого были списаны деньги
        /// </summary>
        public long AccountId { get; set; }

        public decimal PaymentAmount { get; set; }
        public DateTime PaymentDateTime { get; set; }
        public DateOnly PaymentDate { get; set; }
        public PaymentStatusType PaymentStatus { get; set; }
        public PaymentType Type { get; set; }

        //public bool ResponseCame { get; set; }
        //[MemberNotNullWhen(true, nameof(ResponseCame))]
        //public PaymentStatusType? Response { get; set; }

        public Guid CreditId { get; set; }
        public Credit? Credit { get; set; }
    }

    public class IssuingCreditPayment : Payment
    {
        public IssuingCreditPayment()
        {
            Type = PaymentType.IssuingCredit;
        }
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
