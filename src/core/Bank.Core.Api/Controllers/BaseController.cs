using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Bank.Core.Api.Controllers;

[ApiController]
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
    
    
}