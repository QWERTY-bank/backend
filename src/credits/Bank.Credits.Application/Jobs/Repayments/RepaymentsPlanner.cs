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
    public class RepaymentsPlanner : BasePlanner<RepaymentPlan, RepaymentsPlanner>
    {
        public RepaymentsPlanner(
            CreditsDbContext dbContext,
            ILogger<RepaymentsPlanner> logger,
            IOptions<RepaymentsPlannerOptions> options
        ) : base(dbContext, logger, options.Value.CreditsInOneRequest) { }

        protected override IQueryable<Credit> FilterCredits(DbSet<Credit> credits)
        {
            throw new NotImplementedException();
        }

        protected override Task<RepaymentPlan?> GetLastPlanAsync()
        {
            throw new NotImplementedException();
        }

        protected override Task AddPlansAsync(List<RepaymentPlan> newPlans)
        {
            throw new NotImplementedException();
        }

        protected override Task BlockPlansAsync()
            => _dbContext.CloseRepaymentsPlansForUpdate();
    }
}
