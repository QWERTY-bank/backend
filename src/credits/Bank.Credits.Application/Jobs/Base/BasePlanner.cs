using Bank.Credits.Application.Jobs.Helpers;
using Bank.Credits.Domain.Common;
using Bank.Credits.Domain.Jobs;
using Bank.Credits.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Bank.Credits.Application.Jobs.Base
{
    public abstract class BasePlanner<TPlannedEntity, TPlan, TPlanner> : IJob
        where TPlan : PlanBaseEntity, new() where TPlannedEntity : JobPlannedBaseEntity
    {
        protected readonly CreditsDbContext _dbContext;
        protected readonly ILogger<TPlanner> _logger;
        protected readonly int _plannedEntitiesInOneRequest;

        public BasePlanner(
            CreditsDbContext dbContext,
            ILogger<TPlanner> logger,
            int creditsInOneRequest)
        {
            _dbContext = dbContext;
            _logger = logger;
            _plannedEntitiesInOneRequest = creditsInOneRequest;
        }

        protected abstract IQueryable<TPlannedEntity> FilterPlannedEntity();
        protected virtual Task<TPlan?> GetLastPlanAsync() => Task.FromResult<TPlan?>(new() { FromPlanId = -1, ToPlanId = -1 });
        protected abstract Task AddPlansAsync(List<TPlan> newPlans);
        protected abstract Task BlockPlansAsync();
        public async Task Execute(IJobExecutionContext context)
        {
            using var scope = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                await BlockPlansAsync();

                var plannedEntitiesQuery = FilterPlannedEntity();

                var TPlannedEntitiesExists = await plannedEntitiesQuery.AnyAsync();
                if (!TPlannedEntitiesExists)
                {
                    return;
                }

                // Запрашиваем минимальный и максимальный planId кредита со статусом Requested
                var minPlanId = await plannedEntitiesQuery.MinAsync(x => x.PlanId);
                var maxPlanId = await plannedEntitiesQuery.MaxAsync(x => x.PlanId);

                // Запрашиваем последний запрос и берем от туда ToPlanId
                var lastPlan = await GetLastPlanAsync();

                minPlanId = lastPlan == null || lastPlan.ToPlanId + 1 < minPlanId
                    ? minPlanId
                    : lastPlan.ToPlanId + 1;

                if (minPlanId <= maxPlanId)
                {
                    var newPlans = new List<TPlan>();

                    for (var i = 0; i < (maxPlanId - minPlanId + 1) / _plannedEntitiesInOneRequest; i++)
                    {
                        var plan = PlanFabric.CreatePlan<TPlan>(minPlanId, maxPlanId, _plannedEntitiesInOneRequest, i);
                        newPlans.Add(plan);
                    }

                    // Если есть остаток от деления, то добавляем еще один запрос для обработки оставшихся кредитов
                    if ((maxPlanId - minPlanId + 1) % _plannedEntitiesInOneRequest > 0)
                    {
                        var plan = PlanFabric.CreatePlan<TPlan>(minPlanId, maxPlanId, _plannedEntitiesInOneRequest, newPlans.Count);
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
