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
        }
    }
}
