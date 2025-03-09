using Bank.Credits.Application.Jobs.Helpers;
using Bank.Credits.Domain.Jobs;
using Bank.Credits.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Quartz;
using System.Data;

namespace Bank.Credits.Application.Jobs.Base
{
    public abstract class BaseHandler<TPlan, THandler> : IJob where TPlan : PlanBaseEntity, new()
    {
        protected readonly CreditsDbContext _dbContext;
        protected readonly ILogger<THandler> _logger;
        protected readonly int _creditsInOneRequest;

        public BaseHandler(
           CreditsDbContext dbContext,
           ILogger<THandler> logger,
           int creditsInOneRequest)
        {
            _dbContext = dbContext;
            _logger = logger;
            _creditsInOneRequest = creditsInOneRequest;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var creditRequest = await GetPlanAsync();
            if (creditRequest == null)
            {
                return;
            }

            try
            {
                for (var i = 0; i < (creditRequest.ToPlanId - creditRequest.FromPlanId + 1) / _creditsInOneRequest; i++)
                {
                    await HandleCreditsAsync(creditRequest.FromPlanId, creditRequest.ToPlanId, i);
                }

                if ((creditRequest.ToPlanId - creditRequest.FromPlanId + 1) % _creditsInOneRequest > 0)
                {
                    await HandleCreditsAsync(creditRequest.FromPlanId, creditRequest.ToPlanId, (creditRequest.ToPlanId - creditRequest.FromPlanId + 1) / _creditsInOneRequest);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in ${nameof(THandler)}");

                creditRequest.Status = PlanStatusType.Error;
                await _dbContext.SaveChangesAsync();

                return;
            }

            creditRequest.Status = PlanStatusType.Done;
            await _dbContext.SaveChangesAsync();
        }

        protected abstract Task HandlePlannedEntitiesAsync(long fromPlanId, long toPlanId);
        private async Task HandleCreditsAsync(long startFrom, long endAt, long segmentIndex)
        {
            (long fromPlanId, long toPlanId) = PlanFabric.GetSegment(startFrom, endAt, _creditsInOneRequest, segmentIndex);

            using var scope = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);
            try
            {
                await HandlePlannedEntitiesAsync(fromPlanId, toPlanId);

                await _dbContext.SaveChangesAsync();
            }
            finally
            {
                await scope.CommitAsync();
            }
        }

        protected abstract DbSet<TPlan> SelectPlans();
        private async Task<TPlan?> GetPlanAsync()
        {
            var creditRequestQuery = SelectPlans()
               .Where(x => x.Status == PlanStatusType.Wait || x.Status == PlanStatusType.Error ||
                           x.Status == PlanStatusType.InProcess && x.StatusUpdatedAt < DateTime.UtcNow.AddMinutes(-1));

            var creditRequest = await creditRequestQuery.FirstOrDefaultAsync();
            if (creditRequest != null)
            {
                using var scope = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);
                try
                {
                    creditRequest = await creditRequestQuery.FirstOrDefaultAsync(x => x.Id == creditRequest.Id);
                    if (creditRequest == null)
                    {
                        return null;
                    }

                    creditRequest.Status = PlanStatusType.InProcess;
                    creditRequest.StatusUpdatedAt = DateTime.UtcNow;

                    await _dbContext.SaveChangesAsync();
                    await scope.CommitAsync();

                    return creditRequest;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error in GetPlanAsync in {nameof(THandler)}");

                    await _dbContext.Database.RollbackTransactionAsync();
                    throw;
                }
            }

            return null;
        }
    }
}
