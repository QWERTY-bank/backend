using Bank.Credits.Application.Jobs.Base;
using Bank.Credits.Application.Jobs.Repayments.Configurations;
using Bank.Credits.Domain.Common.Helpers;
using Bank.Credits.Domain.Credits;
using Bank.Credits.Domain.Jobs;
using Bank.Credits.Persistence;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Bank.Credits.Application.Jobs.Repayments
{
    public class RepaymentsPlanner : BasePlanner<Credit, RepaymentPlan, RepaymentsPlanner>
    {
        public RepaymentsPlanner(
            CreditsDbContext dbContext,
            ILogger<RepaymentsPlanner> logger,
            IOptions<RepaymentsPlannerOptions> options
        ) : base(dbContext, logger, options.Value.CreditsInOneRequest) { }

        protected override IQueryable<Credit> FilterPlannedEntity()
        {
            // Учитываем только те, у которых сегодня платеж
            return _dbContext.Credits
                .Where(x => x.Status == CreditStatusType.Active)
                .Where(x => x.PaymentsInfo.NextPaymentDate == DateHelper.CurrentDate);
            // Учитываем только те кредиты, которые не имеют завершенного или находящегося в ожидании платежа
            //.Where(x => !x.PaymentHistory!.Any(x => x.PaymentDate == DateHelper.CurrentDate && x.Type == PaymentType.Repayment
            //                                    && (x.PaymentStatus == PaymentStatusType.InProcess || x.PaymentStatus == PaymentStatusType.Conducted)));
        }

        protected override Task AddPlansAsync(List<RepaymentPlan> newPlans)
            => _dbContext.RepaymentPlans.AddRangeAsync(newPlans);

        protected override Task BlockPlansAsync()
            => _dbContext.CloseRepaymentsPlansForUpdate();
    }
}
