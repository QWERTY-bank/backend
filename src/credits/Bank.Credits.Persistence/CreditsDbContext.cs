using Bank.Credits.Domain.Common;
using Bank.Credits.Domain.Tariffs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Reflection;

namespace Bank.Credits.Persistence
{
    public class CreditsDbContext : DbContext
    {
        private const string BankCreditsSchema = "bank_credits";

        public CreditsDbContext(DbContextOptions<CreditsDbContext> options) : base(options)
        {
        }

        public DbSet<Tariff> Tariffs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var assembly = Assembly.GetExecutingAssembly();
            modelBuilder.ApplyConfigurationsFromAssembly(assembly);
            modelBuilder.HasDefaultSchema(BankCreditsSchema);
        }

        #region ovveride SaveChanges methods

        public override int SaveChanges()
        {
            SetTimestamps(ChangeTracker);
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            SetTimestamps(ChangeTracker);
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            SetTimestamps(ChangeTracker);
            return base.SaveChangesAsync(cancellationToken);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            SetTimestamps(ChangeTracker);
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private static void SetTimestamps(ChangeTracker changeTracker)
        {
            var entities = changeTracker.Entries<SoftDeleteBaseEntity>();

            foreach (var entity in entities)
            {
                if (entity.State == EntityState.Deleted)
                {
                    entity.Entity.DeleteDateTime = DateTime.UtcNow;
                    entity.State = EntityState.Modified;
                }
            }
        }

        #endregion
    }
}
