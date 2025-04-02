using Bank.Credits.Application.Jobs.Base;
using Bank.Credits.Application.Jobs.Repayments.Configurations;
using Bank.Credits.Domain.Common.Helpers;
using Bank.Credits.Domain.Credits;
using Bank.Credits.Domain.Jobs;
using Bank.Credits.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Bank.Credits.Application.Jobs.Repayments
{
    /*
     * Аксиомы
     * 1. Платеж проводим один раз в конце периода, то есть в его последний день
     * 2. Период длится CreditConstants.PaymentPeriodDays дней
     * 3. Процент начисляет так же один раз в конце периода, но перед платежом
     * 4. Платеж по кредиту проводится только автоматически
     * 5. Платеж завершившийся ошибкой из-за НЕДОСТАТОЧНОГО БАЛАНСА считается просроченным
     * 6. При просрочке:
     *      a. следующий платеж будет только в следующем периоде
     *      b. пересчитываем платежи с учетом отсутствия просроченного платежа
     *      c. пересчитываем кредитный рейтинг в худшую сторону
     *      d. если последний день кредита уже прошел, то следующий платеж будет в конце следующего периода, который рассчитывается исходя из даты выдачи кредитов, как и предыдущие периоды
     * 7. Если процент начислялся и платеж еще не прошел, но клиент уменьшил сумму кредита (то есть платеж по уменьшению пришел раньше 
     *    платежа по кредиту), то платеж по кредиту должен быть пересчитан с учетом того, что процент за период уже начислен
     * 8. Кредитный рейтинг пересчитывается после каждого успешного платежа или просрочки
     * 9. Если клиент хочет внести платеж вручную (не путать с уменьшением):
     *      a. то текущая дата, должна совпадать с концом периода (то есть с датой платежа)
     *      b. такой платеж убирает просрочку за текущий день платежа (если она есть)
     *      c. пока не планируется реализовывать
     * 10. Каждый платеж или остаток должен иметь НЕ БОЛЕЕ ДВУХ знаков после запятой
     * 11. Если срок, на который взят кредит, не кратен числу дней в периоде, то последний платеж совершается в последний день срока, на который взят кредит
     * 12. Последний платеж может быть не равен остальным платежам, он всегда равен остатку по долгу
     * 13. Платежи обрабатываются в том порядке, в котором они создались

    Сейчас
    1. Задача по планированию взятия кредита и задача по взятию кредита
    2. Задача по планированию создания платежа по кредиту и задача по создания платежа по кредиту
    3. Задача по планированию проведения платежа и задача по проведению платежа

    Что нужно сделать:
    + 1. Перенести логику по кредиту в домен
    + 2. Чтобы понять когда начислялся процент и не начислить его заново добавить дату последнего начисления платежа
    3. Чтобы понять что платеж уже совершен и не совершить его заново, сначала создаем платеж, потом после совершения платежа в той же транзакции к бд удаляем из массива платежей платеж и ставим следующую дату платежа
    
    + 1. Сохраняем в кредите массив платежей + какой остаток с процентом будет перед платежом и ближайшую дату платежа
    2. После каждого платежа по кредиту убираем из массива платеж и остаток и ставим следующую дату платежа + улучшаем кредитный рейтинг
    3. При уменьшении кредита пересчитываем массив платежей и остатков. дата следующего платежа меняется только после платежа по кредиту или просрочке + улучшаем кредитный рейтинг
    4. При просрочке пересчитываем массив платежей и остатков и ставим следующую дату платежа + ухудшаем кредитный рейтинг и сохраняем просрочку в бд
    6. Адаптировать задачи по обработке платежей под очереди
    7. При взятии кредита создавать платеж на пополнение счета, если платеж пройдет не успешно, то кредит отменяется

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
                 .Where(x => x.PaymentsInfo.NextPaymentDate == DateHelper.CurrentDate)
                 .ToListAsync();

            foreach(var credit in credits)
            {
                if (!credit.ApplyInterestRate())
                {
                    throw new ArgumentException("Interest calculation error");
                }

                if (!credit.MakeRepayment())
                {
                    throw new ArgumentException("Loan repayment error");
                }

                credit.PaymentHistory ??= [];

                credit.PaymentHistory.Add(new RepaymentPayment()
                {
                    Key = Guid.NewGuid(),
                    AccountId = credit.AccountId,
                    CreditId = credit.Id,
                    PaymentAmount = credit.PaymentsInfo.NextPayment,
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
