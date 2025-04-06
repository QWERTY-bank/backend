using Bank.Common.Kafka;
using Bank.Common.Kafka.Transfers;
using Bank.Credits.Domain.Credits;
using Bank.Credits.Domain.Jobs;
using Bank.Credits.Persistence;
using Bank.Credits.Quartz.Base;
using Bank.Credits.Quartz.Payments.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Bank.Credits.Quartz.Payments
{
    /*
        При создании платежа на погашение вычитаем из общей суммы выплату, если платеж прошел, то ничего больше не делаем. если не прошел, то возвращаем деньги
        При создании платежа на уменьшение сначала проверяем, что сумма кредита не меньше суммы на уменьшение. также вычиатаем деньги
        
    
        при погашении, мы сначала добавляем проценты, потом списываем бабки, (но при просрочке еще и пересчитываем кредит и возвращаем бабки)
        при уменьшении, мы сначала списываем бабки, потом пересчитываем кредит

        Сначала создали платеж на погашение, и после него платеж на уменьшение, но платежи еще не прошли  Пример: 10000 * 1.1 - 200 - 100
        (невозможен) 1. платеж на погашение отменился, платеж на уменьшение прошел -> процент не трогаем (он остается), возвращаем бабки по погашению
        (невозможен) 2. платеж на погашение прошел, платеж на уменьшение отменился
        (невозможен) 3. платеж на погашение отменился, платеж на уменьшение отменился
        (невозможен) 4. платеж на погашение прошел, платеж на уменьшение прошел -> отдыхаем и ничего не делаем

        Сначала создался платеж на уменьшение, и после на погашение, но платежи еще не прошли. Пример: (10000 - 100) * 1.1 - 200
        + 1. платеж на уменьшение отменился, платеж на погашение прошел -> возвращаем бабки по уменьшению, пересчитываем кредит без учета процента и создаем платеж по кредиту на сумму равную разности получившегося равного платежа и прошедшего платежа на погашение (использовать флаг из пункта d)
        + 2. платеж на уменьшение прошел, платеж на погашение отменился -> просто возвращаем бабки по погашению и если это просрочка пересчитываем кредит
        + 3. платеж на уменьшение отменился, платеж на погашение отменился -> возвращаем бабки по уменьшению, пересчитываем кредит без учета процента и вернуть бабки и если это просрочка пересчитываем кредит 
        + 4. платеж на уменьшение прошел, платеж на погашение прошел -> отдыхаем и ничего не делаем

        Итого:
        + a. Запретить платеж на уменьшение, если в очереди есть платеж на погашение
        + b. При уменьшении делаем пересчет кредита сразу при создании платежа
        c. Платежи обрабатываем в рамках всей кучи, а при получении результата по платежу сначала прикрепляем его к соответствующему платежу, а потом отдельной джобой обрабатываем в том порядке, в котором платежи создались в рамках кредита 
        d. Для пункта 1 и 3, добавить в платеж флаг, который говорит, что создавать ли платеж по разнице из 1 пункта или нет, когда пришел результат по платежу по уменьшению. Такой платеж по разнице должен висеть пока 
        не пройдет (если отмениться, то ставить статус пендинг, также не давать закрыть кредит пока есть такой платеж) если будет стоять этот флаг и платеж прошел, то создаем платеж, если отменился то не создаем
        e. Пересчет по погашению при просрочке делаем при обработке результата платежа
        f. перед совершением операции над обновлением кредита блокируем обновление
        g. Добавить инфу про валюту кредита

        // Платежи обрабатываем в рамках кредита, а не в рамках всех платежей, то есть ищем кредиты с хотя бы одним платежом и обрабатываем платежи там в порядке их создания

        //ИТОГО:
        //если платеж на погашение отменился просто возвращаем бабки, но если это прострочка, то еще и пересчитываем кредит
        //если на уменьшение не прошел так же просто возвращаем бабки
        //любые пересчеты кредита делаем только после успешного платежа 

        //если мы уменьшили платеж, сразу после этого начислялся процент и создался платеж на погашение
        

     */
    public class PaymentsHandler : BaseHandler<PaymentsPlan, PaymentsHandler>
    {
        private readonly ITopicProducer<UnitAccountTransferModel> _producer;

        public PaymentsHandler(
            CreditsDbContext dbContext,
            ILogger<PaymentsHandler> logger,
            IOptions<PaymentsHandlerOptions> options,
            ITopicProducer<UnitAccountTransferModel> producer
        ) : base(dbContext, logger, options.Value.CreditsInOneRequest)
        {
            _producer = producer;
        }

        protected override async Task HandlePlannedEntitiesAsync(long fromPlanId, long toPlanId)
        {
            var payments = await _dbContext.Payments
                .Include(x => x.Credit)
                .Where(x => x.PaymentStatus == PaymentStatusType.InProcess)
                .Where(x => fromPlanId <= x.PlanId && x.PlanId <= toPlanId)
                .OrderBy(x => x.PlanId)
                .ToListAsync();

            foreach (var payment in payments)
            {
                var message = new UnitAccountTransferModel()
                {
                    UserAccountId = payment.AccountId,
                    Type = payment.Type == PaymentType.IssuingCredit ? UnitTransferType.Deposit : UnitTransferType.Withdraw,
                    Key = payment.Key,
                    UnitId = Guid.Parse("a99fb2a5-c52e-4168-8ac9-b28878d3b407"),
                    CurrencyValue = new()
                    {
                        Value = payment.PaymentAmount,
                        Code = payment.Credit.CurrencyCode switch
                        {
                            CurrencyCode.Rub => TransferCurrencyCode.Rub,
                            CurrencyCode.Usd => TransferCurrencyCode.Usd,
                            CurrencyCode.Eur => TransferCurrencyCode.Eur,
                            _ => throw new ArgumentOutOfRangeException()
                        },
                    },
                };

                await _producer.ProduceAsync(payment.Credit!.AccountId.ToString(), message, CancellationToken.None);
            }
        }

        protected override DbSet<PaymentsPlan> SelectPlans()
            => _dbContext.PaymentsPlans;
    }
}
