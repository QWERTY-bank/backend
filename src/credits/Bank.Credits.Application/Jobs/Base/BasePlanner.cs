using Bank.Credits.Application.Jobs.Helpers;
using Bank.Credits.Domain.Credits;
using Bank.Credits.Domain.Jobs;
using Bank.Credits.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Bank.Credits.Application.Jobs.Base
{
    public abstract class BasePlanner<TPlan, TPlanner> : IJob where TPlan : PlanBaseEntity, new()
    {
        protected readonly CreditsDbContext _dbContext;
        protected readonly ILogger<TPlanner> _logger;
        protected readonly int _creditsInOneRequest;

        public BasePlanner(
            CreditsDbContext dbContext,
            ILogger<TPlanner> logger,
            int creditsInOneRequest)
        {
            _dbContext = dbContext;
            _logger = logger;
            _creditsInOneRequest = creditsInOneRequest;
        }

        protected abstract IQueryable<Credit> FilterCredits(DbSet<Credit> credits);
        protected abstract Task<TPlan?> GetLastPlanAsync();
        protected abstract Task AddPlansAsync(List<TPlan> newPlans);
        public async Task Execute(IJobExecutionContext context)
        {
            using var scope = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                await _dbContext.CloseIssuingCreditsPlansForUpdate();

                var creditsQuery = FilterCredits(_dbContext.Credits);

                var creditsExists = await creditsQuery.AnyAsync();
                if (!creditsExists)
                {
                    return;
                }

                // Запрашиваем минимальный и максимальный planId кредита со статусом Requested
                var minPlanId = await creditsQuery.MinAsync(x => x.PlanId);
                var maxPlanId = await creditsQuery.MaxAsync(x => x.PlanId);

                // Запрашиваем последний запрос и берем от туда ToPlanId
                var lastPlan = await GetLastPlanAsync();

                minPlanId = lastPlan == null || lastPlan.ToPlanId + 1 < minPlanId
                    ? minPlanId
                    : lastPlan.ToPlanId + 1;

                if (minPlanId <= maxPlanId)
                {
                    var newPlans = new List<TPlan>();

                    for (var i = 0; i < (maxPlanId - minPlanId + 1) / _creditsInOneRequest; i++)
                    {
                        var plan = PlanFabric.CreatePlan<TPlan>(minPlanId, maxPlanId, _creditsInOneRequest, i);
                        newPlans.Add(plan);
                    }

                    // Если есть остаток от деления, то добавляем еще один запрос для обработки оставшихся кредитов
                    if ((maxPlanId - minPlanId + 1) % _creditsInOneRequest > 0)
                    {
                        var plan = PlanFabric.CreatePlan<TPlan>(minPlanId, maxPlanId, _creditsInOneRequest, newPlans.Count);
                        newPlans.Add(plan);
                    }

                    await AddPlansAsync(newPlans);
                    await _dbContext.SaveChangesAsync();
                }

                await scope.CommitAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error in {nameof(TPlanner)}");
                scope.Rollback();
            }
        }
    }
}
