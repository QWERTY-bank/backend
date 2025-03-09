using AutoMapper;
using Bank.Common.Application.Extensions;
using Bank.Common.Application.Z1all.ExecutionResult.StatusCode;
using Bank.Credits.Application.Constants;
using Bank.Credits.Application.Credits.Helpers;
using Bank.Credits.Application.Credits.Models;
using Bank.Credits.Domain.Credits;
using Bank.Credits.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using X.PagedList;

namespace Bank.Credits.Application.Credits
{
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
                .Include(x => x.PaymentHistory)
                .Where(x => x.UserId == userId)
                .AsQueryable();

            var credits = await creditsQuery.ToPagedListAsync(page, pageSize);

            var result = credits.ToMappedPagedList<Credit, CreditShortDto>(_mapper);

            foreach (var item in result)
            {
                var credit = credits.FirstOrDefault(x => x.Id == item.Id);

                if (!(credit?.TakingDate.HasValue ?? false))
                {
                    continue;
                }

                item.NextPaymentAmount = Math.Round(credit?.CalculateNextPaymentAmount() ?? 0M, 2);
                item.NextPaymentDateOnly = credit?.CalculateNextPaymentDate() ?? DateOnly.MinValue;
            }

            return ExecutionResult<IPagedList<CreditShortDto>>.FromSuccess(result);
        }

        public async Task<ExecutionResult<CreditDto>> GetCreditAsync(Guid creditId, Guid userId)
        {
            var credit = await _context.Credits
                .Include(x => x.PaymentHistory)
                .Include(x => x.Tariff)
                .FirstOrDefaultAsync(x => x.Id == creditId && x.UserId == userId);
            if (credit == null)
            {
                _logger.LogInformation($"Credit with id = '{creditId}' not found");
                return ExecutionResult<CreditDto>.FromNotFound("GetCredit", $"Credit with id = '{creditId}' not found");
            }

            var result = _mapper.Map<CreditDto>(credit);

            if (credit.TakingDate.HasValue)
            {
                result.NextPayments ??= [];

                var nextDate = CreditHelper.CalculateNextPaymentDate(credit);

                while (nextDate < credit.LastDate!.Value)
                {
                    result.NextPayments.Add(new()
                    {
                        PaymentAmount = credit.PaymentsInfo.Payment,
                        PaymentDateOnly = nextDate,
                    });
                    nextDate = nextDate.AddDays(CreditConstants.PaymentPeriodDays);
                }

                result.NextPayments.Add(new()
                {
                    PaymentAmount = Math.Round(credit.PaymentsInfo.LastPayment, 2),
                    PaymentDateOnly = DateHelper.CurrentDate <= credit.LastDate!.Value
                        ? credit.LastDate!.Value
                        : DateHelper.CurrentDate,
                });
            }

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
            var credit = await _context.Credits
              .Include(x => x.PaymentHistory!.Where(x => x.Key == model.Key))
              .FirstOrDefaultAsync(x => x.Id == creditId && x.UserId == userId);
            if (credit == null)
            {
                _logger.LogInformation($"Credit with id = '{creditId}' not found");
                return ExecutionResult.FromNotFound("ReduceCredit", $"Credit with id = '{creditId}' not found");
            }

            if (credit.PaymentHistory?.Any() ?? false)
            {
                _logger.LogInformation($"Payment with key = '{model.Key}' already requested");
                return ExecutionResult.FromBadRequest("ReduceCredit", "Payment already requested");
            }

            await _context.Payments.AddAsync(new ReducePayment()
            {
                Key = model.Key,
                AccountId = credit.AccountId,
                CreditId = credit.Id,
                PaymentAmount = model.Value,
                PaymentDateTime = DateTime.UtcNow,
                PaymentStatus = PaymentStatusType.InProcess,
                PaymentDate = DateHelper.CurrentDate
            });

            await _context.SaveChangesAsync();

            return ExecutionResult.FromSuccess();
        }
    }
}
