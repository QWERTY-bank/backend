using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Bank.Users.Persistence
{
    public class CreditsDbContext : DbContext
    {
        private const string BankCreditsSchema = "bank_credits";

        public CreditsDbContext(DbContextOptions<CreditsDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var assembly = Assembly.GetExecutingAssembly();
            modelBuilder.ApplyConfigurationsFromAssembly(assembly);
            modelBuilder.HasDefaultSchema(BankCreditsSchema);
        }
    }
}
