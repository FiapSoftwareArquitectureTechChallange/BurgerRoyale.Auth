﻿using BurgerRoyale.Auth.Application.Services;
using BurgerRoyale.Auth.Domain.Configurations;
using BurgerRoyale.Auth.Domain.DTO;
using BurgerRoyale.Auth.Domain.Entities;
using BurgerRoyale.Auth.Domain.Enumerators;
using BurgerRoyale.Auth.Domain.Exceptions;
using BurgerRoyale.Auth.Domain.Interface.Services;
using Microsoft.Extensions.Options;
using BC = BCrypt.Net.BCrypt;

namespace BurgerRoyale.Auth.UnitTests.Application.Services
{
    public class AccountServiceTests
    {
        private readonly Mock<IUserService> _userService;
        private readonly Mock<IOptions<JwtConfiguration>> _jwtConfiguration;
        private readonly AccountService _accountService;

        public AccountServiceTests()
        {
            _userService = new Mock<IUserService>();
            _jwtConfiguration = new Mock<IOptions<JwtConfiguration>>();

            _jwtConfiguration
                .Setup(x => x.Value)
                .Returns(new JwtConfiguration()
                {
                    Issuer = "issuer",
                    Audience = "audience",
                    SecretKey = "secret_key_0123456789_9876543210"
                });

            _accountService = new AccountService(_userService.Object, _jwtConfiguration.Object);
        }

        [Theory]
        [InlineData("12345678910", null)]
        [InlineData(null, "email@test.com")]
        [InlineData(null, null)]
        public async Task GivenAuthenticateRequest_WhenUserDoesNotExists_ThenShouldThrowUnauthorizedException(string? cpf, string? email)
        {
            // arrange
            var request = new AuthenticationRequestDTO(
                cpf,
                email,
                "password"
            );

            _userService
                .Setup(x => x.GetByCpfAsync(It.IsAny<string>()))
                .ThrowsAsync(new NotFoundException());

            _userService
                .Setup(x => x.GetByEmailAsync(It.IsAny<string>()))
                .ThrowsAsync(new NotFoundException());

            // act
            Func<Task> task = async () => await _accountService.Authenticate(request);

            // assert
            await task.Should()
                .ThrowAsync<UnauthorizedAccessException>()
                .WithMessage("Usuário não autorizado");
        }

        [Fact]
        public async Task GivenAuthenticateRequest_WhenUserPasswordsDoesNotMatch_ThenShouldThrowUnauthorizedException()
        {
            // arrange
            var cpf = "12345678910";

            var user = new User(
                cpf,
                "Name",
                "email@test.com",
                BC.HashPassword("password"),
                UserRole.Admin
            );

            var request = new AuthenticationRequestDTO(
                cpf,
                null,
                "incorrect_password"
            );

            _userService
                .Setup(x => x.GetByCpfAsync(
                    It.IsAny<string>()
                ))
                .ReturnsAsync(user);

            // act
            Func<Task> task = async () => await _accountService.Authenticate(request);

            // assert
            await task.Should()
                .ThrowAsync<UnauthorizedAccessException>()
                .WithMessage("Usuário não autorizado");
        }

        [Theory]
        [InlineData("12345678910", null)]
        [InlineData(null, "email@test.com")]
        [InlineData(null, null)]
        public async Task GivenAuthenticateRequest_WhenUserCredentialsAreCorrect_ThenShouldReturnTokenAndUserInResponse(string? cpf, string? email)
        {
            // arrange
            var password = "password";

            var user = new User(
                "12345678910",
                "Name",
                "email@test.com",
                BC.HashPassword(password),
                UserRole.Admin
            );

            var request = new AuthenticationRequestDTO(
                cpf,
                email,
                password
            );

            _userService
                .Setup(x => x.GetByCpfAsync(It.IsAny<string>()))
                .ReturnsAsync(user);

            _userService
                .Setup(x => x.GetByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(user);

            // act
            var response = await _accountService.Authenticate(request);

            // assert
            response.AccessToken.Should().NotBeNullOrEmpty();
            response.User.Should().BeEquivalentTo(user.AsDto());
        }
    }
}
