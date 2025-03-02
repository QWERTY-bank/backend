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
                for (var i = 0; i < (creditRequest.ToPlanId - creditRequest.FromPlanId) / _options.CreditsInOneRequest; i++)
                {
                    await HandleCredits(
                        creditRequest.FromPlanId + _options.CreditsInOneRequest * i + (i == 0 ? 0 : 1),
                        creditRequest.FromPlanId + _options.CreditsInOneRequest * (i + 1)
                    );
                }

                if ((creditRequest.ToPlanId - creditRequest.FromPlanId) % _options.CreditsInOneRequest > 0)
                {
                    await HandleCredits(
                        creditRequest.FromPlanId + _options.CreditsInOneRequest * ((creditRequest.ToPlanId - creditRequest.FromPlanId) / _options.CreditsInOneRequest) + 1,
                        creditRequest.ToPlanId
                    );
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

        private async Task HandleCredits(long fromPlanId, long toPlanId)
        {
            using var scope = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                await _dbContext.CloseIssuingCreditsForUpdate(fromPlanId, toPlanId);

                var credits = await _dbContext.Credits
                    .Where(x => x.Status == CreditStatusType.Requested)
                    .Where(x => fromPlanId <= x.PlanId && x.PlanId <= toPlanId)
                    .ToListAsync();

                foreach (var credit in credits)
                {
                    var result = await _coreRequestService.UnitAccountDepositTransfer(new()
                    {
                        Key = credit.Key,
                        CurrencyValues = [
                            new()
                            {
                                Code = CurrencyCode.Rub,
                                Value = credit.DebtAmount
                            }
                        ]
                    });

                    credit.Status = result.IsSuccess ? CreditStatusType.Active : CreditStatusType.Canceled;
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

            var creditRequest = await creditRequestQuery
                .FirstOrDefaultAsync();
            if (creditRequest != null)
            {
                using var scope = await _dbContext.Database.BeginTransactionAsync();
                try
                {
                    await _dbContext.CloseIssuingCreditsPlanForUpdate(creditRequest.Id);

                    creditRequest = await creditRequestQuery
                        .FirstOrDefaultAsync(x => x.Id == creditRequest.Id);
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
