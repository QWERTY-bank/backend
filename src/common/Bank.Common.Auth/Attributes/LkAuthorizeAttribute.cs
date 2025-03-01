using Bank.Common.Auth.Models;
using Bank.Common.Models.Auth;
using Microsoft.AspNetCore.Authorization;

namespace Bank.Common.Auth.Attributes
{
    public class BankAuthorizeAttribute : AuthorizeAttribute
    {
        public RoleType MinimalRoleType { get; set; }

        public BankAuthorizeAttribute()
        {
            SetPolicy(new PolicyModel
            {
                MinimalRoleType = MinimalRoleType,
            });
        }

        private void SetPolicy(PolicyModel model)
        {
            Policy = model.ToString();
        }
    }
}
