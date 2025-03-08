using AutoMapper;
using Bank.Common.Application.Extensions;
using Bank.Common.Application.Z1all.ExecutionResult.StatusCode;
using Bank.Credits.Application.Credits.Models;
using Bank.Credits.Domain.Credits;
using Bank.Credits.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using X.PagedList;

namespace Bank.Credits.Application.Credits
{
    /*
        ...
        + Добавить сервис для запроса токенов
        + Добавить сервис для отправки запросов в другие сервисы
    
        + Добавить кредиту статусы: Запрошен, Активный, Закрыт, Отменен    

        ... 
        ?2 Добавить блокировку доступа к id кредита при его обработке, чтобы не было двойных списаний

        ...
        + При взятии кредита, создаем кредит со статусом Запрошен, туда сохраняем ключ идемпотентности
        ?1 Далее в джобе выполняем пополнение счета пользователя с счета банка. Добавляем платеж + Ставим статус Активный

        ...
        ?+ Добавить планировщик задач, который будет генерировать запросы на начисление процента и списание денег по кредитам
        ?+ Для этого добавить специальный интовый id, который будет уникальным для каждого запроса на начисление процента и списание денег и будет выдаваться в порядке создания кредита
        ?+ Раз в день запускать планировщик, который будет генерировать запросы на начисление процента и списание денег по кредитам, в каждом запросе будет указан отрезок idшников кредитов, которые нужно обработать, также у запроса будет статус

        Нужно убедится что планировщик один на все инстансы сервиса
        Планировщик запускаем раз 10 минут. После запуска смотрим на уже созданные запросы и ориентируемся на последний, чтобы начать создавать запросы от него

        ...
        Добавить джобу для списания денег по кредиту
        Должна запускаться каждую минут и подбирать свободный запрос, сортировать кредиты по дате создания и выбирать со статусом Активный
        Добавить дату последнего платежа по кредиту, менять ее на дату текущего платежа, если 1) его провела джоба 2) Сумма внесенная пользователем больше или равна сумме платежа + оставшуюся часть считать как уменьшение кредита
        Также добавить дату последнего начисления процента, если она не равна дате начисления, то сначала начисляем процент

        ...
        Добавить джобу, которая пройдет по всем кредитам и добавит туда процент. 
        Должна запускаться каждую минут и подбирать свободный запрос, сортировать кредиты по дате создания и выбирать со статусом Активный

        ...
        Начисление процента должно быть в начале дня, а списание в конце дня

        Добавить Константы которые говорят сколько минут длится день и сколько дней длится период в конце которого нужно списать деньги за кредит
        И по этим параметрам запускать джобы

    */

    /*
     
        1. Добавить джобу по начислению процента
        2. Добавить джобу по списанию денег по кредиту
        3. Добавить просмотр истории платежей
        4. Добавить просмотр будущих платежей
        5. Добавить возможность уменьшить кредит
        6. Учитывать константу CreditConstants.DayLength при списании денег

     */
    public class CreditsService : ICreditsService
    {
        private readonly CreditsDbContext _context;
        private readonly ILogger<CreditsService> _logger;
        private readonly IMapper _mapper;

        public CreditsService(
            CreditsDbContext context,
            ILogger<CreditsService> logger,
            IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ExecutionResult<IPagedList<CreditShortDto>>> GetCreditsAsync(CreditsFilter filter, int page, int pageSize, Guid userId)
        {
            var creditsQuery = _context.Credits
                .Where(x => x.UserId == userId)
                .AsQueryable();

            var credits = await creditsQuery.ToPagedListAsync(page, pageSize);

            var result = credits.ToMappedPagedList<Credit, CreditShortDto>(_mapper);
            return ExecutionResult<IPagedList<CreditShortDto>>.FromSuccess(result);
        }

        public async Task<ExecutionResult<CreditDto>> GetCreditAsync(Guid creditId, Guid userId)
        {
            var credit = await _context.Credits
                //.Include(x => x.PaymentHistory)
                .FirstOrDefaultAsync(x => x.Id == creditId && x.UserId == userId);
            if (credit == null)
            {
                _logger.LogInformation($"Credit with id = '{creditId}' not found");
                return ExecutionResult<CreditDto>.FromNotFound("GetCredit", $"Credit with id = '{creditId}' not found");
            }

            var result = _mapper.Map<CreditDto>(credit);

            result.NextPayments =
            [
                new () 
                {
                    PaymentAmount = 100,
                    PaymentDateOnly = DateOnly.FromDateTime(DateTime.Now.AddDays(10))
                },
                new ()
                {
                    PaymentAmount = 100,
                    PaymentDateOnly = DateOnly.FromDateTime(DateTime.Now.AddDays(20))
                },
                new ()
                {
                    PaymentAmount = 99,
                    PaymentDateOnly = DateOnly.FromDateTime(DateTime.Now.AddDays(30))
                }
            ];
            result.PaymentHistory = 
            [
                new() 
                {
                    PaymentAmount = 100,
                    PaymentDateTime = DateTime.Today
                },
                new()
                {
                    PaymentAmount = 321,
                    PaymentDateTime = DateTime.Today.AddDays(1)
                }
            ];

            return ExecutionResult<CreditDto>.FromSuccess(result);
        }

        public async Task<ExecutionResult> TakeCreditAsync(TakeCreditDto model, Guid userId)
        {
            var creditAlreadyRequested = await _context.Credits.AnyAsync(x => x.Key == model.Key);
            if (creditAlreadyRequested)
            {
                _logger.LogInformation($"Credit with key = '{model.Key}' already requested");
                return ExecutionResult.FromBadRequest("TakeCredit", "Credit already requested");
            }

            var tariff = await _context.Tariffs.FirstOrDefaultAsync(x => x.Id == model.TariffId);
            if (tariff == null)
            {
                _logger.LogInformation($"Tariff with id = '{model.TariffId}' not found");
                return ExecutionResult.FromBadRequest("TakeCredit", $"Tariff with id = '{model.TariffId}' not found");
            }

            if (model.PeriodDays < tariff.MinPeriodDays || tariff.MaxPeriodDays < model.PeriodDays)
            {
                _logger.LogInformation($"The number of days must be from {tariff.MinPeriodDays} to {tariff.MaxPeriodDays}");
                return ExecutionResult.FromBadRequest("TakeCredit", $"The number of days must be from {tariff.MinPeriodDays} to {tariff.MaxPeriodDays}");
            }

            var newCredit = _mapper.Map<Credit>(model);

            newCredit.UserId = userId;

            await _context.Credits.AddAsync(newCredit);
            await _context.SaveChangesAsync();

            return ExecutionResult.FromSuccess();
        }

        public async Task<ExecutionResult> ReduceCreditAsync(Guid creditId, ReduceCreditDto model, Guid userId)
        {
            return ExecutionResult.FromSuccess();
        }
    }
}
