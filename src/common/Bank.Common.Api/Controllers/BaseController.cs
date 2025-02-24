using Bank.Common.Api.Attributes;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Bank.Common.Api.Controllers
{
    [ApiController]
    [ValidateModelState]
    public abstract class BaseController : ControllerBase
    {
        protected Guid UserId
        {
            get
            {
                var value = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                return User.Identity?.IsAuthenticated == null || value == null
                    ? Guid.Empty
                    : Guid.Parse(value);
            }
        }

        protected Guid TokenId
        {
            get
            {
                var value = User.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
                return User.Identity?.IsAuthenticated == null || value == null
                    ? Guid.Empty
                    : Guid.Parse(value);
            }
        }
    }
}
