using Bank.Common.Auth.Extensions;
using Bank.Common.Auth.RequirementHandlers.Requirements;
using Microsoft.AspNetCore.Authorization;

namespace Bank.Common.Auth.RequirementHandlers
{
    internal class MinimalRoleTypeHandler : AuthorizationHandler<MinimalRoleTypeRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimalRoleTypeRequirement requirement)
        {
            var maxRole = context.User.GetUserMaxRole();

            if (maxRole >= requirement.MinimalRoleType)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
