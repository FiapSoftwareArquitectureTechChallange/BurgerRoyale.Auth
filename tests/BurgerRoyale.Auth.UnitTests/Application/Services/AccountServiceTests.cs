using BurgerRoyale.Auth.Application.Services;
using BurgerRoyale.Auth.Domain.Configurations;
using BurgerRoyale.Auth.Domain.DTO;
using BurgerRoyale.Auth.Domain.Enumerators;
using BurgerRoyale.Auth.Domain.Exceptions;
using BurgerRoyale.Auth.Domain.Interface.Services;
using BurgerRoyale.Auth.UnitTests.Domain.EntitiesMocks;
using Microsoft.Extensions.Options;

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
                .WithMessage("User not authorized");
        }

        [Fact]
        public async Task GivenAuthenticateRequest_WhenUserPasswordsDoesNotMatch_ThenShouldThrowUnauthorizedException()
        {
            // arrange
            var cpf = "12345678910";

            var user = UserMock.Get(
                cpf,
                "Name",
                "email@test.com",
                "password",
                userRole: UserRole.Admin
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
                .WithMessage("User not authorized");
        }

        [Theory]
        [InlineData("12345678910", null)]
        [InlineData(null, "email@test.com")]
        [InlineData(null, null)]
        public async Task GivenAuthenticateRequest_WhenUserCredentialsAreCorrect_ThenShouldReturnTokenAndUserInResponse(string? cpf, string? email)
        {
            // arrange
            var password = "password";

            var user = UserMock.Get(
                "12345678910",
                "Name",
                "email@test.com",
                "phone",
                "address",
                password,
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

        [Fact]
        public async Task GivenUserRegisterRequest_WhenPasswordDoesNotMatchWithConfirmation_ThenShouldThrowDomainException()
        {
            // arrange
            var request = new CustomerRequestDTO(
                "12345678910",
                "Name",
                "email@test.com",
                "",
                "",
                "password",
                "wrong_password_confirmation"
            );

            // act
            Func<Task> task = async () => await _accountService.RegisterCustomerAsync(request);

            // assert
            await task.Should()
                .ThrowAsync<DomainException>()
                .WithMessage("Wrong passwords");
        }

        [Fact]
        public async Task GivenUserRegisterRequest_WhenCreateUser_ThenShouldCreateAsCustomer()
        {
            // arrange
            var request = new CustomerRequestDTO(
                "12345678910",
                "Name",
                "email@test.com",
                "",
                "",
                "password",
                "password"
            );

            var userDto = UserMock.Get(
                request.Cpf,
                request.Name,
                request.Email,
                request.Password,
                userRole:UserRole.Customer
            ).AsDto();

            _userService
                .Setup(x => x.CreateAsync(It.IsAny<UserCreateRequestDTO>()))
                .ReturnsAsync(userDto);

            // act
            var response = await _accountService.RegisterCustomerAsync(request);

            // assert
            response.Should().BeEquivalentTo(userDto);

            _userService
                .Verify(
                    x => x.CreateAsync(
                        It.Is<UserCreateRequestDTO>(y => 
                            y.Cpf == request.Cpf
                            && y.UserRole == UserRole.Customer
                        )
                    ),
                    Times.Once
                );
        }

        [Fact]
        public async Task GivenUpdateAccountRequest_WhenPasswordDoesNotMatchWithConfirmation_ThenShouldThrowDomainException()
        {
            // arrange
            var request = new AccountUpdateRequestDTO(
                "Name",
                "email@test.com",
                "",
                "",
                "12345678910",
                "password",
                "wrong_password_confirmation"
            );

            // act
            Func<Task> task = async () => await _accountService.UpdateAccountAsync(Guid.NewGuid(), request);

            // assert
            await task.Should()
                .ThrowAsync<DomainException>()
                .WithMessage("Wrong passwords");
        }

        [Fact]
        public async Task GivenUpdateAccountRequest_WhenCurrentPasswordDoesNotMatch_ThenShouldThrowUnauthorizedException()
        {
            // arrange
            var request = new AccountUpdateRequestDTO(
                "Name",
                "email@test.com",
                "",
                "",
                "current_password",
                "password",
                "password"
            );

            var user = UserMock.Get(password: "another_password");

            _userService
                .Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(user);

            // act
            Func<Task> task = async () => await _accountService.UpdateAccountAsync(Guid.NewGuid(), request);

            // assert
            await task.Should()
                .ThrowAsync<UnauthorizedAccessException>()
                .WithMessage("Current password incorrect");
        }

        [Fact]
        public async Task GivenUpdateAccountRequest_WhenUserUpdated_ThenShouldReturnDto()
        {
            // arrange
            var userId = Guid.NewGuid();
            var currentPassword = "current_password";

            var request = new AccountUpdateRequestDTO(
                "Name",
                "email@test.com",
                "",
                "",
                currentPassword,
                "password",
                "password"
            );

            var user = UserMock.Get(password: currentPassword);

            _userService
                .Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(user);

            _userService
                .Setup(x => x.UpdateAsync(
                    It.IsAny<Guid>(),
                    It.IsAny<UserUpdateRequestDTO>()))
                .ReturnsAsync(user.AsDto());

            // act
            var response = await _accountService.UpdateAccountAsync(userId, request);

            // assert
            response.Should().BeOfType<UserDTO>();

            _userService
                .Verify(
                    x => x.UpdateAsync(
                        userId,
                        It.Is<UserUpdateRequestDTO>(y =>
                            y.Name == request.Name
                            && y.Email == request.Email
                            && y.Password == request.NewPassword
                        )
                    ),
                    Times.Once
                );
        }

        [Fact]
        public async Task GivenUnregisterRequested_WhenUserUnregistered_ThenUserShouldBeUnregistered()
        {
            // arrange
            var userId = Guid.NewGuid();

            _userService
                .Setup(x => x.DeleteAsync(It.IsAny<Guid>()))
                .Returns(Task.CompletedTask);

            // act
            await _accountService.UnregisterAsync(userId);

            // assert
            _userService
                .Verify(
                    x => x.DeleteAsync(userId),
                    Times.Once
                );
        }
    }
}