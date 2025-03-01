using Bank.Common.Models.Auth;

namespace Bank.Users.Persistence
{
    public class RolesSeed
    {
        private readonly UsersDbContext _context;

        public RolesSeed(UsersDbContext context)
        {
            _context = context;
        }

        public void SeedRoles()
        {
            var roles = _context.Roles.ToList();

            foreach (var roleType in Enum.GetValues<RoleType>())
            {
                if (!roles.Any(x => x.Type == roleType))
                {
                    _context.Roles.Add(new()
                    {
                        RoleName = roleType.ToString(),
                        Type = roleType
                    });
                }
            }

            _context.SaveChanges();
        }
    }
}
