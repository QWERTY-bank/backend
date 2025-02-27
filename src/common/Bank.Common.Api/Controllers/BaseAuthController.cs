using Bank.Common.Api.Attributes;
using Bank.Common.Auth.Attributes;
using Bank.Common.Auth.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Common.Api.Controllers
{
    [ApiController]
    [ValidateModelState]
    [BankAuthorize]
    public abstract class BaseAuthController : ControllerBase
    {
        protected Guid UserId { get => User.GetUserId(); }

        protected Guid TokenJTI { get => User.GetTokenId(); }
    }
}
