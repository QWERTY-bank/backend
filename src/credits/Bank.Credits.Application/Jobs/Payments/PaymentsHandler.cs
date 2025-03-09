using Bank.Credits.Application.Jobs.Base;
using Bank.Credits.Application.Jobs.Payments.Configurations;
using Bank.Credits.Domain.Jobs;
using Bank.Credits.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Bank.Credits.Application.Jobs.Payments
{
    public class PaymentsHandler : BaseHandler<PaymentsPlan, PaymentsHandler>
    {
        public PaymentsHandler(
           CreditsDbContext dbContext,
           ILogger<PaymentsHandler> logger,
           IOptions<PaymentsHandlerOptions> options
       ) : base(dbContext, logger, options.Value.CreditsInOneRequest) { }

        protected override Task HandleCreditsAsync(long fromPlanId, long toPlanId)
        {
            throw new NotImplementedException();
        }

        protected override DbSet<PaymentsPlan> SelectPlans()
            => _dbContext.PaymentsPlans;
    }
}
