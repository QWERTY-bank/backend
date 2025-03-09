using Bank.Credits.Application.Credits.Helpers;
using Bank.Credits.Application.Jobs.Base;
using Bank.Credits.Application.Jobs.Repayments.Configurations;
using Bank.Credits.Domain.Credits;
using Bank.Credits.Domain.Jobs;
using Bank.Credits.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Bank.Credits.Application.Jobs.Repayments
{
    /*
        1. Добавить джобу по обработке платежей
        2. Добавить джобу по созданию платежей на погашение, чтобы сохранять ключ идемпонтентности 

        В джобе по созданию погашений создаем платеж с текущей инфой по кредиту
        В джобе по обработке платежа перед этим заново расчитываем платеж, так как перед этим пользователь мог уменьшить сумму кредита

        Обработка платежа будет проходить 5 на 1, 5 - период запуска планера, 1 - период запуска хендлера
        Обработка создания погашения тоже 5 на 1. Запрашивать нужно будет те кредиты у которых нет платежа в статусе ВПроцессе или Выполнен в день платежа

        В джобе по созданию погашений добавить начисление проценка и в кредит поставить последнюю дату начисления процента и перед начислением стравнивать текущую дату в выставленной, если не равна то начисляем процент


        Добавить сервис который будет в зависимости от CreditConstants.DayLength переводить часы в день и заменить получение текущей даты на него
    */
    public class RepaymentsHandler : BaseHandler<RepaymentPlan, RepaymentsHandler>
    {
        public RepaymentsHandler(
           CreditsDbContext dbContext,
           ILogger<RepaymentsHandler> logger,
           IOptions<RepaymentsHandlerOptions> options
        ) : base(dbContext, logger, options.Value.CreditsInOneRequest) { }

        protected override async Task HandlePlannedEntitiesAsync(long fromPlanId, long toPlanId)
        {
            var credits = await _dbContext.Credits
                .Include(x => x.PaymentHistory)
                .Include(x => x.Tariff)
                .Where(x => x.Status == CreditStatusType.Active)
                .Where(x => !x.PaymentHistory!.Any(x => x.PaymentDate == DateHelper.CurrentDate && x.Type == PaymentType.Repayment
                                                    && (x.PaymentStatus == PaymentStatusType.InProcess || x.PaymentStatus == PaymentStatusType.Conducted)))
                .Where(x => fromPlanId <= x.PlanId && x.PlanId <= toPlanId)
                .ToListAsync();

            foreach (var credit in credits)
            {
                // Проверяем, что у кредита сегодня платеж 
                var nextPaymentDate = credit.CalculateNextPaymentDate();
                if (nextPaymentDate != DateHelper.CurrentDate)
                {
                    continue;
                }

                credit.PaymentHistory ??= [];

                credit.PaymentHistory.Add(new RepaymentPayment()
                {
                    Key = Guid.NewGuid(),
                    AccountId = credit.AccountId,
                    CreditId = credit.Id,
                    PaymentAmount = credit.CalculateNextPaymentAmount(),
                    PaymentDate = DateHelper.CurrentDate,
                    PaymentDateTime = DateTime.UtcNow,
                    PaymentStatus = PaymentStatusType.InProcess
                });
            }
        }

        protected override DbSet<RepaymentPlan> SelectPlans()
            => _dbContext.RepaymentPlans;
    }
}
