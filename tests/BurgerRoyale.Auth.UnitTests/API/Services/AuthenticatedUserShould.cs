using BurgerRoyale.Auth.API.Services;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace BurgerRoyale.Auth.UnitTests.API.Services;

public class AuthenticatedUserShould
{
    [Fact]
    public void Get_Authenticated_User_Id()
    {
        // Arrange

        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();

        var userId = Guid.NewGuid();

        var claim = new Claim(ClaimTypes.NameIdentifier, userId.ToString());

        var identity = new ClaimsIdentity([ claim ]);

        var claimPrincipal = new ClaimsPrincipal(identity);
        
        httpContextAccessorMock
            .Setup(hca => hca.HttpContext!.User)
            .Returns(claimPrincipal);

        var authenticatedUser = new AuthenticatedUser(httpContextAccessorMock.Object);

        // Act

        Guid authenticatedUserId = authenticatedUser.Id;

        // Assert

        Assert.Equal(userId, authenticatedUserId);
    }
}