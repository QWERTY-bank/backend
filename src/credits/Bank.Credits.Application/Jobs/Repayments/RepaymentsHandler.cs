using Bank.Credits.Application.Jobs.Base;
using Bank.Credits.Application.Jobs.Repayments.Configurations;
using Bank.Credits.Domain.Jobs;
using Bank.Credits.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Bank.Credits.Application.Jobs.Repayments
{
    public class RepaymentsHandler : BaseHandler<RepaymentPlan, RepaymentsHandler>
    {
        public RepaymentsHandler(
           CreditsDbContext dbContext,
           ILogger<RepaymentsHandler> logger,
           IOptions<RepaymentsHandlerOptions> options
        ) : base(dbContext, logger, options.Value.CreditsInOneRequest) { }

        protected override Task HandleCreditsAsync(long fromPlanId, long toPlanId)
        {
            throw new NotImplementedException();
        }

        protected override DbSet<RepaymentPlan> SelectPlans()
        {
            throw new NotImplementedException();
        }
    }
}
