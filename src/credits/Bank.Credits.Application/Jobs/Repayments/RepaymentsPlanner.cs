using Bank.Credits.Application.Jobs.Base;
using Bank.Credits.Application.Jobs.Repayments.Configurations;
using Bank.Credits.Domain.Credits;
using Bank.Credits.Domain.Jobs;
using Bank.Credits.Persistence;
using Microsoft.EntityFrameworkCore;
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
            throw new NotImplementedException();
        }

        protected override Task<RepaymentPlan?> GetLastPlanAsync()
        {
            return _dbContext.RepaymentPlans
                .OrderByDescending(x => x.Id)
                .FirstOrDefaultAsync();
        }

        protected override Task AddPlansAsync(List<RepaymentPlan> newPlans)
            => _dbContext.RepaymentPlans.AddRangeAsync(newPlans);

        protected override Task BlockPlansAsync()
            => _dbContext.CloseRepaymentsPlansForUpdate();
    }
}
