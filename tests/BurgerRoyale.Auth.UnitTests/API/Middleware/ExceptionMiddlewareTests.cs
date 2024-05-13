using BurgerRoyale.Auth.API.Middleware;
using BurgerRoyale.Auth.Domain.Exceptions;
using Microsoft.AspNetCore.Http;

namespace BurgerRoyale.Auth.UnitTests.API.Middleware;

public class ExceptionMiddlewareTests
{
    private readonly Mock<RequestDelegate> _requestDelegate;
    private readonly Mock<HttpContext> _context;

    private readonly ExceptionMiddleware _middleware;

    public ExceptionMiddlewareTests()
    {
        _requestDelegate = new Mock<RequestDelegate>();
        _context = new Mock<HttpContext>();

        var httpResponse = new Mock<HttpResponse>();

        _context
            .Setup(x => x.Response)
            .Returns(httpResponse.Object);

        _middleware = new ExceptionMiddleware(_requestDelegate.Object);
    }

    [Fact]
    public async Task GivenRequest_WhenInvoke_ThenShouldNotThrowException()
    {
        // arrange
        _requestDelegate
            .Setup(x => x.Invoke(It.IsAny<HttpContext>()))
            .Returns(Task.CompletedTask);

        // act
        Func<Task> task = async () => await _middleware.Invoke(_context.Object);

        // assert
        await task.Should().NotThrowAsync<Exception>();
    }

    [Fact]
    public async Task GivenRequest_WhenThrowExceptionOnInvoke_ThenShouldThrowException()
    {
        // arrange
        _requestDelegate
            .Setup(x => x.Invoke(It.IsAny<HttpContext>()))
            .ThrowsAsync(new Exception("Exception message"));

        // act
        Func<Task> task = async () => await _middleware.Invoke(_context.Object);

        // assert
        await task.Should().ThrowAsync<Exception>();
    }

    [Fact]
    public async Task GivenRequest_WhenThrowDomainExceptionOnInvoke_ThenShouldThrowException()
    {
        // arrange
        _requestDelegate
            .Setup(x => x.Invoke(It.IsAny<HttpContext>()))
            .ThrowsAsync(new DomainException("Exception message"));

        // act
        Func<Task> task = async () => await _middleware.Invoke(_context.Object);

        // assert
        await task.Should().ThrowAsync<Exception>();
    }

    [Fact]
    public async Task GivenRequest_WhenThrowNotFoundExceptionOnInvoke_ThenShouldThrowException()
    {
        // arrange
        _requestDelegate
            .Setup(x => x.Invoke(It.IsAny<HttpContext>()))
            .ThrowsAsync(new NotFoundException("Exception message"));

        // act
        Func<Task> task = async () => await _middleware.Invoke(_context.Object);

        // assert
        await task.Should().ThrowAsync<Exception>();
    }

    [Fact]
    public async Task GivenRequest_WhenThrowUnauthorizedAccessExceptionOnInvoke_ThenShouldThrowException()
    {
        // arrange
        _requestDelegate
            .Setup(x => x.Invoke(It.IsAny<HttpContext>()))
            .ThrowsAsync(new UnauthorizedAccessException("Exception message"));

        // act
        Func<Task> task = async () => await _middleware.Invoke(_context.Object);

        // assert
        await task.Should().ThrowAsync<Exception>();
    }
}
