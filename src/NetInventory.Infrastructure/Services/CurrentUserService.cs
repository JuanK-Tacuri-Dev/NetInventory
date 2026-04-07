using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using NetInventory.Application.Common.Interfaces;

namespace NetInventory.Infrastructure.Services;

public sealed class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    public string GetCurrentUser()
        => httpContextAccessor.HttpContext?.User.Identity?.Name ?? "system";

    public string GetCurrentUserId()
        => httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "system";
}
