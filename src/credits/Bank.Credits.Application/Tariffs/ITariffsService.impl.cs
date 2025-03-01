using AutoMapper;
using Bank.Common.Application.Extensions;
using Bank.Credits.Application.Tariffs.Models;
using Bank.Credits.Domain.Tariffs;
using Bank.Credits.Persistence;
using Bank.Credits.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using X.PagedList;
using Z1all.ExecutionResult.StatusCode;

namespace Bank.Credits.Application.Tariffs
{
    public class TariffsService : ITariffsService
    {
        private readonly IMapper _mapper;
        private readonly CreditsDbContext _context;
        private readonly ILogger<TariffsService> _logger;

        public TariffsService(
            IMapper mapper,
            CreditsDbContext context,
            ILogger<TariffsService> logger)
        {
            _mapper = mapper;
            _context = context;
            _logger = logger;
        }

        public async Task<ExecutionResult<IPagedList<TariffDto>>> GetTariffsAsync(TariffsFilters filters, int page, int pageSize)
        {
            var tariffsQuery = _context.Tariffs
                .GetUndeleted()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filters.Name))
            {
                tariffsQuery = tariffsQuery.Where(x => x.Name.Contains(filters.Name));
            }

            if (filters.MaxInterestRate.HasValue)
            {
                tariffsQuery = tariffsQuery.Where(x => x.InterestRate <= filters.MaxInterestRate);
            }

            if (filters.InterestRateType.HasValue)
            {
                tariffsQuery = tariffsQuery.Where(x => x.InterestRateType == filters.InterestRateType);
            }

            if (filters.PeriodDays.HasValue)
            {
                tariffsQuery = tariffsQuery.Where(x => x.MinPeriodDays <= filters.PeriodDays && filters.PeriodDays <= x.MaxPeriodDays);
            }

            var tariffs = await tariffsQuery.ToPagedListAsync(page, pageSize);

            var result = tariffs.ToMappedPagedList<Tariff, TariffDto>(_mapper);
            return ExecutionResult<IPagedList<TariffDto>>.FromSuccess(result);
        }

        public async Task<ExecutionResult> CreateTariffAsync(CreateTariffDto model)
        {
            var nameExists = await _context.Tariffs
                .GetUndeleted()
                .AnyAsync(x => x.Name == model.Name.Trim());
            if (nameExists)
            {
                _logger.LogInformation($"Tariff with the same name '{model.Name.Trim()}' already exists.");
                return ExecutionResult.FromBadRequest("CreateTariff", $"Tariff with the same name '{model.Name.Trim()}' already exists.");
            }

            var tariff = _mapper.Map<Tariff>(model);

            await _context.Tariffs.AddAsync(tariff);

            await _context.SaveChangesAsync();

            return ExecutionResult.FromSuccess();
        }

        public async Task<ExecutionResult> DeleteTariffAsync(Guid tariffId)
        {
            var tariff = await _context.Tariffs
                .GetUndeleted()
                .FirstOrDefaultAsync(x => x.Id == tariffId);
            if (tariff == null)
            {
                _logger.LogInformation($"Tariff with id = '{tariffId} does not found.");
                return ExecutionResult.FromNotFound("DeleteTariff", $"Tariff with id = '{tariffId}' does not found.");
            }

            _context.Tariffs.Remove(tariff);

            await _context.SaveChangesAsync();

            return ExecutionResult.FromSuccess();
        }
    }
}
