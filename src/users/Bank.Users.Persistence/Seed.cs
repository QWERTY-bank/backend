using Bank.Common.Models.Auth;

namespace Bank.Users.Persistence
{
    public static class Seed
    {
        public static void SeedData(this UsersDbContext context)
        {
            context.SeedRoles();
        }

        private static void SeedRoles(this UsersDbContext context)
        {
            var roles = context.Roles.ToList();

            foreach (var roleType in Enum.GetValues<RoleType>())
            {
                if (!roles.Any(x => x.Type == roleType))
                {
                    context.Roles.Add(new()
                    {
                        RoleName = roleType.ToString(),
                        Type = roleType
                    });
                }
            }

            context.SaveChanges();
        }
    }
}
