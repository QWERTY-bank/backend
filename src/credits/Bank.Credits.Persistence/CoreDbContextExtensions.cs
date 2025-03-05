using Microsoft.EntityFrameworkCore;

namespace Bank.Credits.Persistence
{
    public static class CreditDbContextExtensions
    {
        public static Task<int> CloseIssuingCreditsPlansForUpdate(this CreditsDbContext dbContext)
        {
            var command = "LOCK TABLE bank_plans.\"IssuingCreditsPlans\" IN ACCESS EXCLUSIVE MODE";
            return dbContext.Database.ExecuteSqlRawAsync(command);
        }
    }
}

