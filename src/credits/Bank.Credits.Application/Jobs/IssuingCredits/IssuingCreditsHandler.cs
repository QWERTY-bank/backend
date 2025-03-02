using Bank.Credits.Persistence;
using Quartz;

namespace Bank.Credits.Application.Jobs.IssuingCredits
{
    public class IssuingCreditsHandler : IJob
    {
        private readonly CreditsDbContext _dbContext;

        public IssuingCreditsHandler(CreditsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task Execute(IJobExecutionContext context)
        {
            throw new NotImplementedException();
        }
    }
}
