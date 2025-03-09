using Bank.Credits.Application.Jobs.Base;
using Bank.Credits.Application.Jobs.IssuingCredits.Configurations;
using Bank.Credits.Domain.Credits;
using Bank.Credits.Domain.Jobs;
using Bank.Credits.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Bank.Credits.Application.Jobs.IssuingCredits
{
    public class IssuingCreditsPlanner : BasePlanner<IssuingCreditsPlan, IssuingCreditsPlanner>
    {
        public IssuingCreditsPlanner(
            CreditsDbContext dbContext,
            ILogger<IssuingCreditsPlanner> logger,
            IOptions<IssuingCreditsPlannerOptions> options
        ) : base(dbContext, logger, options.Value.CreditsInOneRequest) { }

        protected override IQueryable<Credit> FilterCredits(DbSet<Credit> credits)
            => credits.Where(x => x.Status == CreditStatusType.Requested);

        protected override Task<IssuingCreditsPlan?> GetLastPlanAsync()
        {
            return _dbContext.IssuingCreditsPlans
                .OrderByDescending(x => x.Id)
                .FirstOrDefaultAsync();
        }

        protected override Task AddPlansAsync(List<IssuingCreditsPlan> newPlans)
            => _dbContext.IssuingCreditsPlans.AddRangeAsync(newPlans);

        protected override Task BlockPlansAsync()
            => _dbContext.CloseIssuingCreditsPlansForUpdate();
    }
}
