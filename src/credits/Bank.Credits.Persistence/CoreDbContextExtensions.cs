using Microsoft.EntityFrameworkCore;

namespace Bank.Credits.Persistence
{
    public static class CreditDbContextExtensions
    {
        // TODO: Добавить блокировку кредитов + добавить фильтр на статус кредита Requested
        public static Task<int> CloseIssuingCreditsForUpdate(this CreditsDbContext dbContext, long fromPlanId, long toPlanId)
        {
            throw new NotImplementedException();
        }

        public static Task<int> CloseIssuingCreditsPlanForUpdate(this CreditsDbContext dbContext, long planId)
        {
            var command = $"SELECT * FROM bank_plans.IssuingCreditsPlans WHERE id IN ({planId}) AND is_closed = false FOR UPDATE;";
            return dbContext.Database.ExecuteSqlRawAsync(command);
        }

        public static Task<int> CloseIssuingCreditsPlansForUpdate(this CreditsDbContext dbContext)
        {
            var command = "LOCK TABLE bank_plans.IssuingCreditsPlans IN ACCESS EXCLUSIVE MODE";
            return dbContext.Database.ExecuteSqlRawAsync(command);
        }
    }
}

