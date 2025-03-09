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
    public class PaymentsPlanner : BasePlanner<PaymentsPlan, PaymentsPlanner>
    {
        public PaymentsPlanner(
            CreditsDbContext dbContext,
            ILogger<PaymentsPlanner> logger,
            IOptions<PaymentsPlannerOptions> options
        ) : base(dbContext, logger, options.Value.CreditsInOneRequest) { }

        protected override IQueryable<Credit> FilterCredits(DbSet<Credit> credits)
        {
            throw new NotImplementedException();
        }

        protected override Task<PaymentsPlan?> GetLastPlanAsync()
        {
            throw new NotImplementedException();
        }

        protected override Task AddPlansAsync(List<PaymentsPlan> newPlans)
        {
            throw new NotImplementedException();
        }

        protected override Task BlockPlansAsync()
        {
            throw new NotImplementedException();
        }
    }
}
