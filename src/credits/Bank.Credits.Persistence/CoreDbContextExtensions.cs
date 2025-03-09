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

        public static Task<int> ClosePaymentsPlansForUpdate(this CreditsDbContext dbContext)
        {
            var command = "LOCK TABLE bank_plans.\"PaymentsPlans\" IN ACCESS EXCLUSIVE MODE";
            return dbContext.Database.ExecuteSqlRawAsync(command);
        }

        public static Task<int> CloseRepaymentsPlansForUpdate(this CreditsDbContext dbContext)
        {
            var command = "LOCK TABLE bank_plans.\"RepaymentPlans\" IN ACCESS EXCLUSIVE MODE";
            return dbContext.Database.ExecuteSqlRawAsync(command);
        }
    }
}

