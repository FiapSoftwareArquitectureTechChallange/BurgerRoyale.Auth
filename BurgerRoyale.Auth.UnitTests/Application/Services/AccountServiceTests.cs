using BurgerRoyale.Auth.Application.Services;
using BurgerRoyale.Auth.Domain.DTO;
using BurgerRoyale.Auth.Domain.Entities;
using BurgerRoyale.Auth.Domain.Enumerators;
using BurgerRoyale.Auth.Domain.Interface.Repositories;
using System.Linq.Expressions;
using BC = BCrypt.Net.BCrypt;

namespace BurgerRoyale.Auth.UnitTests.Application.Services
{
    public class AccountServiceTests
    {
        private readonly Mock<IUserRepository> _userRepository;
        private readonly AccountService _accountService;

        public AccountServiceTests()
        {
            _userRepository = new Mock<IUserRepository>();

            _accountService = new AccountService(_userRepository.Object);
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
    }
}
