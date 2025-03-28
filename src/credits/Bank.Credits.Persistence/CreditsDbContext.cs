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
        public DbSet<Payment> Payments { get; set; }

        #endregion

        #region Plans

        public DbSet<IssuingCreditsPlan> IssuingCreditsPlans { get; set; }
        public DbSet<PaymentsPlan> PaymentsPlans { get; set; }
        public DbSet<RepaymentPlan> RepaymentPlans { get; set; }

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
                .OwnsOne(x => x.PaymentsInfo, nav =>
                {
                    nav.Property(y => y.DebtsWithInterest)
                       .HasConversion(
                            v => string.Join(';', v),
                            v => new(v.Split(';', StringSplitOptions.RemoveEmptyEntries).Select(decimal.Parse).ToList())
                       );
                });

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

            modelBuilder.Entity<Payment>()
                .ToTable(nameof(Payments));
            modelBuilder.Entity<Payment>()
                .HasOne(x => x.Credit)
                .WithMany(x => x.PaymentHistory)
                .HasForeignKey(x => x.CreditId)
                .IsRequired();
            modelBuilder.Entity<Payment>()
                .Property(x => x.PlanId)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Payment>()
                .HasIndex(x => x.PlanId);

            #endregion

            #region Plans

            modelBuilder.ConfigurePlanEntity<IssuingCreditsPlan>(nameof(IssuingCreditsPlans), BankCreditsPlanSchema);
            modelBuilder.ConfigurePlanEntity<PaymentsPlan>(nameof(PaymentsPlans), BankCreditsPlanSchema);
            modelBuilder.ConfigurePlanEntity<RepaymentPlan>(nameof(RepaymentPlans), BankCreditsPlanSchema);

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
