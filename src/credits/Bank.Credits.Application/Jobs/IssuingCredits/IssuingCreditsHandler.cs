using Bank.Credits.Application.Jobs.Base;
using Bank.Credits.Application.Jobs.IssuingCredits.Configurations;
using Bank.Credits.Application.Requests;
using Bank.Credits.Application.Requests.Models;
using Bank.Credits.Domain.Common.Helpers;
using Bank.Credits.Domain.Credits;
using Bank.Credits.Domain.Jobs;
using Bank.Credits.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Bank.Credits.Application.Jobs.IssuingCredits
{
    public class IssuingCreditsHandler : BaseHandler<IssuingCreditsPlan, IssuingCreditsHandler>
    {
        private readonly ICoreRequestService _coreRequestService;

        public IssuingCreditsHandler(
            CreditsDbContext dbContext,
            ICoreRequestService coreRequestService,
            IOptions<IssuingCreditsHandlerOptions> options,
            ILogger<IssuingCreditsHandler> logger
        ) : base(dbContext, logger, options.Value.CreditsInOneRequest)
        {
            _coreRequestService = coreRequestService;
        }

        protected override async Task HandlePlannedEntitiesAsync(long fromPlanId, long toPlanId)
        {
            //var credits = await _dbContext.Credits
            //    .Include(x => x.Tariff)
            //    .Include(x => x.PaymentHistory)
            //    .Where(x => x.Status == CreditStatusType.Requested)
            //    .Where(x => fromPlanId <= x.PlanId && x.PlanId <= toPlanId)
            //    .ToListAsync();

            //foreach (var credit in credits)
            //{
            //    var result = await _coreRequestService.UnitAccountDepositTransferAsync(new()
            //    {
            //        Key = credit.Key,
            //        CurrencyValues =
            //        [
            //            new()
            //            {
            //                Code = CurrencyCode.Rub,
            //                Value = credit.DebtAmount
            //            }
            //        ]
            //    }, credit.AccountId);

            //    credit.Status = result.IsSuccess ? CreditStatusType.Active : CreditStatusType.Canceled;
            //    if (credit.Status == CreditStatusType.Active)
            //    {
            //        credit.TakingDate = DateHelper.CurrentDate;
            //        credit.UpdateCreditPaymentsInfo();
            //    }
            //}
        }

        protected override DbSet<IssuingCreditsPlan> SelectPlans()
            => _dbContext.IssuingCreditsPlans;
    }
}
