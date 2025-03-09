using Bank.Credits.Application.Credits.Helpers;
using Bank.Credits.Application.Jobs.Base;
using Bank.Credits.Application.Jobs.Payments.Configurations;
using Bank.Credits.Application.Requests;
using Bank.Credits.Application.Requests.Models;
using Bank.Credits.Domain.Credits;
using Bank.Credits.Domain.Jobs;
using Bank.Credits.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Bank.Credits.Application.Jobs.Payments
{
    public class PaymentsHandler : BaseHandler<PaymentsPlan, PaymentsHandler>
    {
        private readonly ICoreRequestService _coreRequestService;

        public PaymentsHandler(
           CreditsDbContext dbContext,
           ILogger<PaymentsHandler> logger,
           ICoreRequestService coreRequestService,
           IOptions<PaymentsHandlerOptions> options
        ) : base(dbContext, logger, options.Value.CreditsInOneRequest)
        {
            _coreRequestService = coreRequestService;
        }

        protected override async Task HandlePlannedEntitiesAsync(long fromPlanId, long toPlanId)
        {
            var payments = await _dbContext.Payments
                .Include(x => x.Credit)
                    .ThenInclude(x => x!.Tariff)
                .Where(x => x.PaymentStatus == PaymentStatusType.InProcess)
                .Where(x => fromPlanId <= x.PlanId && x.PlanId <= toPlanId)
                .ToListAsync();

            foreach (var payment in payments)
            {
                switch (payment.Type)
                {
                    case PaymentType.ReduceDebt:
                        await HandleReduceDebtAsync(payment);
                        break;
                    case PaymentType.Repayment:
                        await HandleRepaymentAsync(payment);
                        break;
                };
            }
        }

        private async Task HandleReduceDebtAsync(Payment payment)
        {
            if (payment.PaymentAmount > payment.Credit!.DebtAmount || payment.Credit.Status != CreditStatusType.Active)
            {
                payment.PaymentStatus = PaymentStatusType.Canceled;
                return;
            }

            await TransferAsync(payment);

            if (payment.PaymentStatus == PaymentStatusType.Conducted)
            {
                payment.Credit!.ReduceDebt(payment.PaymentAmount);
            }
        }

        private async Task HandleRepaymentAsync(Payment payment)
        {
            if (payment.Credit!.Status != CreditStatusType.Active)
            {
                payment.PaymentStatus = PaymentStatusType.Canceled;
                return;
            }

            payment.PaymentAmount = payment.Credit!.CalculateNextPaymentAmount();

            await TransferAsync(payment);

            if (payment.PaymentStatus == PaymentStatusType.Conducted)
            {
                payment.Credit.ApplyInterestRate();
                payment.Credit!.MakePayment(payment.PaymentAmount);
            }
        }

        private async Task TransferAsync(Payment payment)
        {
            var result = await _coreRequestService.UnitAccountWithdrawTransferAsync(new()
            {
                Key = payment.Key,
                CurrencyValues =
                [
                    new()
                    {
                        Code = CurrencyCode.Rub,
                        Value = payment.PaymentAmount,
                    }
                ]       
            }, payment.AccountId);

            payment.PaymentStatus = result.IsSuccess ? PaymentStatusType.Conducted : PaymentStatusType.Canceled;
        }

        protected override DbSet<PaymentsPlan> SelectPlans()
            => _dbContext.PaymentsPlans;
    }
}
