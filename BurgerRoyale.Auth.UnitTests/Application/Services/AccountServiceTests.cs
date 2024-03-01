using BurgerRoyale.Auth.Application.Services;
using BurgerRoyale.Auth.Domain.Configurations;
using BurgerRoyale.Auth.Domain.DTO;
using BurgerRoyale.Auth.Domain.Entities;
using BurgerRoyale.Auth.Domain.Enumerators;
using BurgerRoyale.Auth.Domain.Interface.Repositories;
using Microsoft.Extensions.Options;
using System.Linq.Expressions;
using BC = BCrypt.Net.BCrypt;

namespace BurgerRoyale.Auth.UnitTests.Application.Services
{
    public class AccountServiceTests
    {
        private readonly Mock<IUserRepository> _userRepository;
        private readonly Mock<IOptions<JwtConfiguration>> _jwtConfiguration;
        private readonly AccountService _accountService;

        public AccountServiceTests()
        {
            _userRepository = new Mock<IUserRepository>();
            _jwtConfiguration = new Mock<IOptions<JwtConfiguration>>();

            _jwtConfiguration
                .Setup(x => x.Value)
                .Returns(new JwtConfiguration()
                {
                    Issuer = "issuer",
                    Audience = "audience",
                    SecretKey = "secret_key_0123456789_9876543210"
                });

            _accountService = new AccountService(_userRepository.Object, _jwtConfiguration.Object);
        }

        [Fact]
        public async Task GivenAuthenticateRequest_WhenUserDoesNotExists_ThenShouldThrowUnauthorizedException()
        {
            // arrange
            var cpf = "12345678910";

            var request = new AuthenticationRequestDTO(
                cpf,
                null,
                "password"
            );

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

            _userRepository
                .Setup(x => x.FindFirstDefaultAsync(
                    It.IsAny<Expression<Func<User, bool>>>()
                ))
                .ReturnsAsync(user);

            // act
            Func<Task> task = async () => await _accountService.Authenticate(request);

            // assert
            await task.Should()
                .ThrowAsync<UnauthorizedAccessException>()
                .WithMessage("Usuário não autorizado");
        }

        [Fact]
        public async Task GivenAuthenticateRequest_WhenUserCredentialsAreCorrect_ThenShouldReturnTokenAndUserInResponse()
        {
            // arrange
            var cpf = "12345678910";
            var password = "password";

            var user = new User(
                cpf,
                "Name",
                "email@test.com",
                BC.HashPassword(password),
                UserRole.Admin
            );

            var request = new AuthenticationRequestDTO(
                cpf,
                null,
                password
            );

            _userRepository
                .Setup(x => x.FindFirstDefaultAsync(
                    It.IsAny<Expression<Func<User, bool>>>()
                ))
                .ReturnsAsync(user);

            // act
            var response = await _accountService.Authenticate(request);

            // assert
            response.AccessToken.Should().NotBeNullOrEmpty();
            response.User.Should().BeEquivalentTo(new UserDTO(user));
        }
    }
}
