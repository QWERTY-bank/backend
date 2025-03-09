using Bank.Credits.Domain.Jobs;
using Microsoft.EntityFrameworkCore;

namespace Bank.Credits.Persistence
{
    public static class PlanExtensions
    {
        public static void ConfigurePlanEntity<TPlanEntity>(this ModelBuilder modelBuilder, string schema)
            where TPlanEntity : PlanBaseEntity
        {
            modelBuilder.Entity<TPlanEntity>()
                .ToTable(nameof(TPlanEntity), schema);
            modelBuilder.Entity<TPlanEntity>()
                .HasKey(x => x.Id);
            modelBuilder.Entity<TPlanEntity>()
                .Property(x => x.Status)
                .HasDefaultValue(PlanStatusType.Wait);
            modelBuilder.Entity<TPlanEntity>()
                .HasIndex(x => x.Id)
                .IsUnique();
        }
    }
}
