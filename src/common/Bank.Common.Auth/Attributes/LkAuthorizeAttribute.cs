﻿using Bank.Common.Auth.Models;
using Bank.Common.Models.Auth;
using Microsoft.AspNetCore.Authorization;

namespace Bank.Common.Auth.Attributes
{
    public class BankAuthorizeAttribute : AuthorizeAttribute
    {
        public BankAuthorizeAttribute()
        {
            SetPolicy(new PolicyModel());
        }

        public BankAuthorizeAttribute(RoleType minimalRoleType, string? authenticationSchemes = null)
        {
            AuthenticationSchemes = authenticationSchemes;
            SetPolicy(new PolicyModel
            {
                MinimalRoleType = minimalRoleType,
            });
        }

        private void SetPolicy(PolicyModel model)
        {
            Policy = model.ToString();
        }
    }
}
