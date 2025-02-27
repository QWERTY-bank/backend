using Bank.Common.Auth.RequirementHandlers.Requirements;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Bank.Common.Models.Auth;

namespace Bank.Common.Auth.Models
{
    internal class PolicyModel
    {
        public RoleType? MinimalRoleType { get; set; }

        public IEnumerable<IAuthorizationRequirement> GetRequirements()
        {
            yield return new DenyAnonymousAuthorizationRequirement();

            if (MinimalRoleType.HasValue)
            {
                yield return new MinimalRoleTypeRequirement(MinimalRoleType.Value);
            }
        }
    }
}
