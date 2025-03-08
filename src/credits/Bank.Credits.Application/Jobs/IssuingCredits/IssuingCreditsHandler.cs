using Bank.Credits.Application.Credits.Helpers;
using Bank.Credits.Application.Jobs.Helpers;
using Bank.Credits.Application.Jobs.IssuingCredits.Configurations;
using Bank.Credits.Application.Requests;
using Bank.Credits.Application.Requests.Models;
using Bank.Credits.Domain.Credits;
using Bank.Credits.Domain.Jobs;
using Bank.Credits.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Quartz;
using System.Data;

namespace Bank.Credits.Application.Jobs.IssuingCredits
{
    public class IssuingCreditsHandler : IJob
    {
        private readonly CreditsDbContext _dbContext;
        private readonly ILogger<IssuingCreditsHandler> _logger;
        private readonly ICoreRequestService _coreRequestService;
        private readonly IssuingCreditsHandlerOptions _options;

        public IssuingCreditsHandler(
            CreditsDbContext dbContext,
            ICoreRequestService coreRequestService,
            IOptions<IssuingCreditsHandlerOptions> options,
            ILogger<IssuingCreditsHandler> logger)
        {
            _dbContext = dbContext;
            _coreRequestService = coreRequestService;
            _options = options.Value;
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var creditRequest = await GetIssuingCreditsPlanAsync();
            if (creditRequest == null)
            {
                return;
            }

            try
            {
                for (var i = 0; i < (creditRequest.ToPlanId - creditRequest.FromPlanId + 1) / _options.CreditsInOneRequest; i++)
                {
                    await HandleCredits(creditRequest.FromPlanId, creditRequest.ToPlanId, i);
                }

                if ((creditRequest.ToPlanId - creditRequest.FromPlanId + 1) % _options.CreditsInOneRequest > 0)
                {
                    await HandleCredits(creditRequest.FromPlanId, creditRequest.ToPlanId, (creditRequest.ToPlanId - creditRequest.FromPlanId + 1) / _options.CreditsInOneRequest);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in IssuingCreditsHandler");

                creditRequest.Status = PlanStatusType.Error;
                await _dbContext.SaveChangesAsync();

                return;
            }

            creditRequest.Status = PlanStatusType.Done;
            await _dbContext.SaveChangesAsync();
        }

        private async Task HandleCredits(long startFrom, long endAt, long segmentIndex)
        {
            (long fromPlanId, long toPlanId) = PlanFabric.GetSegment(startFrom, endAt, _options.CreditsInOneRequest, segmentIndex);

            using var scope = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);
            try
            {
                var credits = await _dbContext.Credits
                    .Include(x => x.Tariff)
                    .Include(x => x.PaymentHistory)
                    .Where(x => x.Status == CreditStatusType.Requested)
                    .Where(x => fromPlanId <= x.PlanId && x.PlanId <= toPlanId)
                    .ToListAsync();

                foreach (var credit in credits)
                {
                    var result = await _coreRequestService.UnitAccountDepositTransferAsync(new()
                    {
                        Key = credit.Key,
                        CurrencyValues = [
                            new()
                            {
                                Code = CurrencyCode.Rub,
                                Value = credit.DebtAmount
                            }
                        ]
                    }, credit.AccountId);

                    credit.Status = result.IsSuccess ? CreditStatusType.Active : CreditStatusType.Canceled;
                    credit.TakingDate = CreditHelper.CurrentDate;
                    credit.UpdateCreditPaymentsInfo();
                }

                await _dbContext.SaveChangesAsync();
            }
            finally
            {
                await scope.CommitAsync();
            }
        }

        private async Task<IssuingCreditsPlan?> GetIssuingCreditsPlanAsync()
        {
            var creditRequestQuery = _dbContext.IssuingCreditsPlans
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
                    _logger.LogError(ex, "Error in GetIssuingCreditsPlanAsync");    

                    await _dbContext.Database.RollbackTransactionAsync();
                    throw;
                }
            }

            return null;
        }
    }
}
