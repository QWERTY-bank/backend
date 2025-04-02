using AutoMapper;
using Bank.Common.Application.Extensions;
using Bank.Common.Application.Z1all.ExecutionResult.StatusCode;
using Bank.Credits.Application.Credits.Models;
using Bank.Credits.Application.Requests;
using Bank.Credits.Application.User;
using Bank.Credits.Domain.Common.Helpers;
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
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly ICoreRequestService _requestService;

        public CreditsService(
            CreditsDbContext context,
            ILogger<CreditsService> logger,
            IUserService userService,
            IMapper mapper,
            ICoreRequestService requestService)
        {
            _context = context;
            _logger = logger;
            _userService = userService;
            _mapper = mapper;
            _requestService = requestService;
        }

        public async Task<ExecutionResult<IPagedList<CreditShortDto>>> GetCreditsAsync(CreditsFilter filter, int page, int pageSize, Guid userId)
        {
            var creditsQuery = _context.Credits
                .Include(x => x.PaymentHistory)
                .Where(x => x.UserId == userId)
                .AsQueryable();

            var credits = await creditsQuery.ToPagedListAsync(page, pageSize);

            var result = credits.ToMappedPagedList<Credit, CreditShortDto>(_mapper);

            return ExecutionResult<IPagedList<CreditShortDto>>.FromSuccess(result);
        }

        public async Task<ExecutionResult<CreditDto>> GetCreditAsync(Guid creditId, Guid userId)
        {
            var credit = await _context.Credits
                .Include(x => x.PaymentHistory.Where(x => x.Type != PaymentType.IssuingCredit))
                .Include(x => x.Tariff)
                .FirstOrDefaultAsync(x => x.Id == creditId && x.UserId == userId);
            if (credit == null)
            {
                _logger.LogInformation($"Credit with id = '{creditId}' not found");
                return ExecutionResult<CreditDto>.FromNotFound("GetCredit", $"Credit with id = '{creditId}' not found");
            }

            var result = _mapper.Map<CreditDto>(credit);

            return ExecutionResult<CreditDto>.FromSuccess(result);
        }

        public async Task<ExecutionResult> TakeCreditAsync(TakeCreditDto model, Guid userId)
        {
            var creditAlreadyRequested = await _context.Payments.AnyAsync(x => x.Key == model.Key);
            if (creditAlreadyRequested)
            {
                _logger.LogInformation($"Credit with payment with key = '{model.Key}' already requested");
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

            var accountInfo = await _requestService.GetAccountBalanceAsync(model.AccountId);
            if (accountInfo.IsNotSuccess)
            {
                return ExecutionResult.FromError(accountInfo);
            }

            if (accountInfo.Result.UserId != userId)
            {
                _logger.LogInformation("Account does not belong to the user");
                return ExecutionResult.FromBadRequest("TakeCredit", "Account does not belong to the user");
            }

            if (accountInfo.Result.IsClosed)
            {
                _logger.LogInformation("Account is closed");
                return ExecutionResult.FromBadRequest("TakeCredit", "Account is closed");
            }

            var newCredit = _mapper.Map<Credit>(model);
            newCredit.CurrencyCode = accountInfo.Result.CurrencyValue.Code;
            newCredit.PaymentHistory = 
            [
                new IssuingCreditPayment() 
                {
                    Key = model.Key,
                    AccountId = model.AccountId,
                    PaymentAmount = model.LoanAmount,
                    PaymentDateTime = DateTime.UtcNow,
                    PaymentStatus = PaymentStatusType.InProcess,
                    PaymentDate = DateHelper.CurrentDate,
                }
            ];

            var user = await _userService.GetUserEntityAsync(userId);
            newCredit.UserId = user.Id;

            await _context.Credits.AddAsync(newCredit);
            await _context.SaveChangesAsync();

            return ExecutionResult.FromSuccess();
        }

        public async Task<ExecutionResult> ReduceCreditAsync(Guid creditId, ReduceCreditDto model, Guid userId)
        {
            var credit = await _context.Credits
              .Include(x => x.PaymentHistory)
              .FirstOrDefaultAsync(x => x.Id == creditId && x.UserId == userId);
            if (credit == null)
            {
                _logger.LogInformation($"Credit with id = '{creditId}' not found");
                return ExecutionResult.FromNotFound("ReduceCredit", $"Credit with id = '{creditId}' not found");
            }

            if (credit.Status != CreditStatusType.Active)
            {
                _logger.LogInformation($"Credit with id = '{creditId}' must have the Active status");
                return ExecutionResult.FromNotFound("ReduceCredit", $"Credit with id = '{creditId}' must have the Active status");
            }

            if (credit.PaymentHistory?.Any(x => x.Key == model.Key) ?? false)
            {
                _logger.LogInformation($"Payment with key = '{model.Key}' already requested");
                return ExecutionResult.FromBadRequest("ReduceCredit", "Payment already requested");
            }

            if (credit.PaymentHistory?.Any(x => x.PaymentStatus == PaymentStatusType.InProcess && x.Type == PaymentType.Repayment) ?? false)
            {
                _logger.LogInformation($"Please try again later.");
                return ExecutionResult.FromBadRequest("ReduceCredit", "Please try again later.");
            }

            if (credit.PaymentsInfo.DebtAmount < model.Value)
            {
                _logger.LogInformation("The amount owed is less than the specified amount");
                return ExecutionResult.FromBadRequest("ReduceCredit", "The amount owed is less than the specified amount");
            }

            credit.ReduceDebt(model.Value);

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
