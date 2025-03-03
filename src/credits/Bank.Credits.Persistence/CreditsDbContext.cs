using Bank.Credits.Domain.Common;
using Bank.Credits.Domain.Credits;
using Bank.Credits.Domain.Jobs;
using Bank.Credits.Domain.Tariffs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Reflection;

namespace Bank.Credits.Persistence
{
    public class CreditsDbContext : DbContext
    {
        private const string BankCreditsSchema = "bank_credits";
        private const string BankCreditsPlanSchema = "bank_plans";

        public CreditsDbContext(DbContextOptions<CreditsDbContext> options) : base(options)
        {
        }

        #region Credits

        public DbSet<Tariff> Tariffs { get; set; }
        public DbSet<Credit> Credits { get; set; }

        #endregion

        #region Plans

        public DbSet<IssuingCreditsPlan> IssuingCreditsPlans { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var assembly = Assembly.GetExecutingAssembly();
            modelBuilder.ApplyConfigurationsFromAssembly(assembly);
            modelBuilder.HasDefaultSchema(BankCreditsSchema);

            #region Credits

            modelBuilder.Entity<Tariff>()
                .ToTable(nameof(Tariffs));

            modelBuilder.Entity<Credit>()
                .ToTable(nameof(Credits));   
            modelBuilder.Entity<Credit>()
                .HasOne(x => x.Tariff)
                .WithMany()
                .HasForeignKey(x => x.TariffId)
                .IsRequired();
            modelBuilder.Entity<Credit>()
                .Property(x => x.PlanId)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Credit>()
                .HasIndex(x => x.PlanId);

            #endregion

            #region Plans

            modelBuilder.Entity<IssuingCreditsPlan>()
                .ToTable(nameof(IssuingCreditsPlans), BankCreditsPlanSchema);
            modelBuilder.Entity<IssuingCreditsPlan>()
                .HasKey(x => x.Id);
            modelBuilder.Entity<IssuingCreditsPlan>()
                .Property(x => x.Status)
                .HasDefaultValue(PlanStatusType.Wait);
            modelBuilder.Entity<IssuingCreditsPlan>()
                .HasIndex(x => x.Id)
                .IsUnique();

            #endregion
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
