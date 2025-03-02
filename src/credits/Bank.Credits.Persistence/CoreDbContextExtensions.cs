using Microsoft.EntityFrameworkCore;

namespace Bank.Credits.Persistence
{
    public static class CreditDbContextExtensions
    {
        public static Task<int> SelectCreditsForUpdate(this CreditsDbContext dbContext, List<Guid> creditIds)
        {
            var creditIdsString = string.Join(",", creditIds);

            var command = $"SELECT id FROM bank_credits.Credits WHERE id IN ({creditIdsString}) AND is_closed = false FOR UPDATE;";
            return dbContext.Database.ExecuteSqlRawAsync(command);
        }

        public static Task<int> CloseIssuingCreditsPlansForUpdate(this CreditsDbContext dbContext)
        {
            var command = "LOCK TABLE bank_plans.IssuingCreditsPlans IN ACCESS EXCLUSIVE MODE";
            return dbContext.Database.ExecuteSqlRawAsync(command);
        }
    }
}

