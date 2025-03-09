using Bank.Credits.Application.Jobs.Base;
using Bank.Credits.Application.Jobs.Payments.Configurations;
using Bank.Credits.Domain.Credits;
using Bank.Credits.Domain.Jobs;
using Bank.Credits.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Bank.Credits.Application.Jobs.Payments
{
    public class PaymentsPlanner : BasePlanner<Payment, PaymentsPlan, PaymentsPlanner>
    {
        public PaymentsPlanner(
            CreditsDbContext dbContext,
            ILogger<PaymentsPlanner> logger,
            IOptions<PaymentsPlannerOptions> options
        ) : base(dbContext, logger, options.Value.CreditsInOneRequest) { }

        protected override IQueryable<Payment> FilterPlannedEntity()
            => _dbContext.Payments.Where(x => x.PaymentStatus == PaymentStatusType.InProcess);

        protected override Task<PaymentsPlan?> GetLastPlanAsync()
        {
            return _dbContext.PaymentsPlans
                .OrderByDescending(x => x.Id)
                .FirstOrDefaultAsync();
        }

        protected override Task AddPlansAsync(List<PaymentsPlan> newPlans)
            => _dbContext.PaymentsPlans.AddRangeAsync(newPlans);

        protected override Task BlockPlansAsync()
            => _dbContext.ClosePaymentsPlansForUpdate();
    }
}
