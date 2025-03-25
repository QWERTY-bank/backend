using Bank.Users.Domain.Settings;
using Bank.Users.Domain.Users;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Bank.Users.Persistence
{
    public class UsersDbContext : DbContext
    {
        private const string BankUsersSchema = "bank_users";

        public UsersDbContext(DbContextOptions<UsersDbContext> options) : base(options)
        {
        }

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<ClientUserSettings> ClientUserSettings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var assembly = Assembly.GetExecutingAssembly();
            modelBuilder.ApplyConfigurationsFromAssembly(assembly);
            modelBuilder.HasDefaultSchema(BankUsersSchema);

            modelBuilder.Entity<UserEntity>()
                .Property(x => x.IsBlocked)
                .HasDefaultValue(false);
            modelBuilder.Entity<UserEntity>()
                .HasMany(x => x.Roles)
                .WithMany();

            modelBuilder.Entity<ClientUserSettings>()
                .HasOne(x => x.User)
                .WithOne()
                .HasForeignKey<ClientUserSettings>(x => x.UserId);
            modelBuilder.Entity<ClientUserSettings>()
                .Property(x => x.HiddenAccounts)
                .HasConversion(
                    v => string.Join(',', v),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray()
                );
        }
    }
}
