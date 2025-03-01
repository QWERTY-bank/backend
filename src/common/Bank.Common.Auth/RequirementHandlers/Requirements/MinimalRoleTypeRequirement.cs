using Bank.Common.Models.Auth;
using Microsoft.AspNetCore.Authorization;

namespace Bank.Common.Auth.RequirementHandlers.Requirements
{
    internal class MinimalRoleTypeRequirement : IAuthorizationRequirement
    {
        public RoleType MinimalRoleType { get; set; }

        public MinimalRoleTypeRequirement(RoleType roleType)
        {
            MinimalRoleType = roleType;
        }
    }
}
