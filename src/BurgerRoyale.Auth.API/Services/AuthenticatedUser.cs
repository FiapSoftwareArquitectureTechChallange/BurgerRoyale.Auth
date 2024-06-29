using BurgerRoyale.Auth.Domain.Interface.Services;
using System.Security.Claims;

namespace BurgerRoyale.Auth.API.Services;

public class AuthenticatedUser(IHttpContextAccessor httpContextAccessor) : IAuthenticatedUser
{
    private readonly ClaimsPrincipal _authenticatedUser = httpContextAccessor.HttpContext!.User;

    public Guid Id => Guid.Parse(_authenticatedUser.FindFirst(ClaimTypes.NameIdentifier)!.Value);
}