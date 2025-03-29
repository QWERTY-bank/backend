using AutoMapper;
using Bank.Common.Application.Z1all.ExecutionResult.StatusCode;
using Bank.Credits.Application.User.Models;
using Bank.Credits.Domain.Credits;
using Bank.Credits.Domain.User;
using Bank.Credits.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Bank.Credits.Application.User
{
    public class UserService : IUserService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly CreditsDbContext _context;
        private readonly IMapper _mapper;

        public UserService(
            IServiceScopeFactory serviceScopeFactory, 
            CreditsDbContext context,
            IMapper mapper)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _context = context;
            _mapper = mapper;
        }

        public async Task<UserEntity> GetUserEntityAsync(Guid userId)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            {
                var _context = scope.ServiceProvider.GetRequiredService<CreditsDbContext>();

                var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
                if (user != null)
                {
                    return user;
                }

                user = new() { Id = userId };

                await _context.Users.AddAsync(user);

                await _context.SaveChangesAsync();

                return user;
            }
        }

        public async Task<ExecutionResult<UserCreditInfoDto>> GetUserCreditInfoAsync(Guid userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null)
            {
                throw new Exception("User not found");
            }

            if (!user.RatingIsActual)
            {
                user.Rating = await CalculateRatingAsync(userId);
                user.RatingIsActual = true;

                await _context.SaveChangesAsync();
            }

            return _mapper.Map<UserCreditInfoDto>(user);
        }

        private async Task<double> CalculateRatingAsync(Guid userId)
        {
            var creditQuery = _context.Credits.Where(x => x.UserId == userId);

            var countActiveCredits = await creditQuery.CountAsync(x => x.Status == CreditStatusType.Active);
            var countClosedCredits = await creditQuery.CountAsync(x => x.Status == CreditStatusType.Closed);

            var amountActiveCredits = await creditQuery
                .Where(x => x.Status == CreditStatusType.Active)
                .SumAsync(x => x.PaymentsInfo.DebtAmount);

            var overdueCount = await _context.Payments
                .CountAsync(x => x.Credit!.UserId == userId && x.PaymentStatus == PaymentStatusType.Overdue);

            var countCreditsCoeff = countActiveCredits * 300.0 / (countActiveCredits + countClosedCredits + 1);
            var countOverdueCoeff = overdueCount * 50.0;
            var debtAmountCoeff = (double)(amountActiveCredits * 200.0M / (amountActiveCredits + 10000.0M));
            return Math.Max(0, Math.Min(1000.0, 1000 - countCreditsCoeff - countOverdueCoeff - debtAmountCoeff));
        }   

        public async Task<ExecutionResult> RecalculateRating(Guid userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            user.RatingIsActual = false;

            await _context.SaveChangesAsync();

            return ExecutionResult.FromSuccess();
        }
    }
}
