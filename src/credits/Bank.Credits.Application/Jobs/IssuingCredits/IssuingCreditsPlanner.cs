using Bank.Credits.Application.Jobs.IssuingCredits.Configurations;
using Bank.Credits.Domain.Credits;
using Bank.Credits.Domain.Jobs;
using Bank.Credits.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Quartz;

namespace Bank.Credits.Application.Jobs.IssuingCredits
{
    public class IssuingCreditsPlanner : IJob
    {
        private readonly CreditsDbContext _dbContext;
        private readonly ILogger<IssuingCreditsPlanner> _logger;
        private IssuingCreditsPlannerOptions _options;

        public IssuingCreditsPlanner(
            CreditsDbContext dbContext,
            ILogger<IssuingCreditsPlanner> logger,
            IOptions<IssuingCreditsPlannerOptions> options)
        {
            _dbContext = dbContext;
            _logger = logger;
            _options = options.Value;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            using var scope = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                await _dbContext.CloseIssuingCreditsPlansForUpdate();

                var creditsQuery = _dbContext.Credits
                    .Where(x => x.Status == CreditStatusType.Requested)
                    .Select(x => x.PlanId);

                // Запрашиваем минимальный и максимальный planId кредита со статусом Requested
                var minPlanId = await creditsQuery.MinAsync();
                var maxPlanId = await creditsQuery.MaxAsync();

                // Запрашиваем последний запрос и берем от туда ToPlanId
                var lastIssuingCreditsPlan = await _dbContext.IssuingCreditsPlans
                    .OrderByDescending(x => x.Id)
                    .FirstOrDefaultAsync();

                minPlanId = lastIssuingCreditsPlan == null || lastIssuingCreditsPlan.ToPlanId < minPlanId
                    ? minPlanId
                    : lastIssuingCreditsPlan.ToPlanId;

                if (minPlanId < maxPlanId)
                {
                    var issuingCreditsPlans = new List<IssuingCreditsPlan>();

                    for (var i = 0; i < (maxPlanId - minPlanId) / _options.CreditsInOneRequest; i++)
                    {
                        issuingCreditsPlans.Add(new IssuingCreditsPlan
                        {
                            FromPlanId = minPlanId + i * _options.CreditsInOneRequest, // TODO: Проверить правильность расчета
                            ToPlanId = minPlanId + (i + 1) * _options.CreditsInOneRequest,
                        });
                    }

                    // Если есть остаток от деления, то добавляем еще один запрос для обработки оставшихся кредитов
                    if ((maxPlanId - minPlanId) % _options.CreditsInOneRequest > 0)
                    {
                        issuingCreditsPlans.Add(new IssuingCreditsPlan
                        {
                            FromPlanId = minPlanId + issuingCreditsPlans.Count * _options.CreditsInOneRequest,
                            ToPlanId = maxPlanId,
                        });
                    }

                    await _dbContext.IssuingCreditsPlans.AddRangeAsync(issuingCreditsPlans);
                    await _dbContext.SaveChangesAsync();
                }

                await scope.CommitAsync(); 
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error in IssuingCreditsPlanner");
                scope.Rollback();
            }
        }
    }
}
