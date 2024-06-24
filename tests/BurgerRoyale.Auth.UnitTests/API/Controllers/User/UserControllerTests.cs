using BurgerRoyale.Auth.API.Controllers.User;
using BurgerRoyale.Auth.Domain.DTO;
using BurgerRoyale.Auth.Domain.Enumerators;
using BurgerRoyale.Auth.Domain.Interface.Services;
using BurgerRoyale.Auth.UnitTests.Domain.EntitiesMocks;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BurgerRoyale.Auth.UnitTests.API.Controllers.User;

public class UserControllerTests
{
    private readonly Mock<IUserService> _userService;
    private readonly UserController _controller;

    public UserControllerTests()
    {
        _userService = new Mock<IUserService>();

        _controller = new UserController(_userService.Object);
    }

    [Theory]
    [InlineData(UserRole.Employee)]
    [InlineData(UserRole.Customer)]
    public async Task GivenUserType_WhenGetUsersDto_ThenShouldReturnUsersDtoList(UserRole userType)
    {
        // arrange
        var usersList = UserMock.GetDtoList(3, userType);

        _userService
            .Setup(x => x.GetUsersDtoAsync(userType))
            .ReturnsAsync(usersList);

        // act
        var response = await _controller.GetUsers(userType) as ObjectResult;

        // assert
        response?.StatusCode.Should().Be((int)HttpStatusCode.OK);

        response?.Value.Should().BeEquivalentTo(usersList);
    }

    [Fact]
    public async Task GivenUserId_WhenGetUserDto_ThenShouldReturnUserDto()
    {
        // arrange
        var user = UserMock.Get();
        var userDto = user.AsDto();

        _userService
            .Setup(x => x.GetDtoByIdAsync(user.Id))
            .ReturnsAsync(userDto);

        // act
        var response = await _controller.GetUser(user.Id) as ObjectResult;

        // assert
        response?.StatusCode.Should().Be((int)HttpStatusCode.OK);

        response?.Value.Should().BeEquivalentTo(userDto);
    }

    [Fact]
    public async Task GivenRequestUser_WhenCreateUser_ThenShouldReturnUserDto()
    {
        // arrange
        var user = UserMock.Get();

        var requestUser = new UserCreateRequestDTO(
            user.Cpf,
            user.Name,
            user.Email,
            user.Phone,
            user.Address,
            user.PasswordHash,
            user.UserRole
        );

        var userDto = user.AsDto();

        _userService
            .Setup(x => x.CreateAsync(requestUser))
            .ReturnsAsync(userDto);

        // act
        var response = await _controller.CreateUser(requestUser) as ObjectResult;

        // assert
        response?.StatusCode.Should().Be((int)HttpStatusCode.Created);

        response?.Value.Should().BeEquivalentTo(userDto);
    }

    [Fact]
    public async Task GivenRequestUser_WhenUpdateUser_ThenShouldReturnUserDto()
    {
        // arrange
        var user = UserMock.Get();

        var requestUser = new UserUpdateRequestDTO(
            user.Name,
            user.Email,
            user.Phone,
            user.Address,
            user.PasswordHash,
            user.UserRole
        );

        var userDto = user.AsDto();

        _userService
            .Setup(x => x.UpdateAsync(user.Id, requestUser))
            .ReturnsAsync(userDto);

        // act
        var response = await _controller.UpdateUser(user.Id, requestUser) as ObjectResult;

        // assert
        response?.StatusCode.Should().Be((int)HttpStatusCode.OK);

        response?.Value.Should().BeEquivalentTo(userDto);
    }

    [Fact]
    public async Task GivenUserId_WhenDeleteUser_ThenShouldReturnNoContent()
    {
        // arrange
        var userId = Guid.NewGuid();

        // act
        var response = await _controller.DeleteUser(userId) as ObjectResult;

        // assert
        response?.StatusCode.Should().Be((int)HttpStatusCode.NoContent);

        _userService.Verify(
            x => x.DeleteAsync(userId),
            Times.Once
        );
    }
}
