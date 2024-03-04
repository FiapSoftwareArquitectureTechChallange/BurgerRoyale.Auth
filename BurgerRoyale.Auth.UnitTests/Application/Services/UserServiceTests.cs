using BurgerRoyale.Auth.Application.Services;
using BurgerRoyale.Auth.Domain.DTO;
using BurgerRoyale.Auth.Domain.Entities;
using BurgerRoyale.Auth.Domain.Enumerators;
using BurgerRoyale.Auth.Domain.Exceptions;
using BurgerRoyale.Auth.Domain.Helpers;
using BurgerRoyale.Auth.Domain.Interface.Repositories;
using BurgerRoyale.Auth.UnitTests.Domain.EntitiesMocks;
using System.Linq.Expressions;

namespace BurgerRoyale.Auth.UnitTests.Application.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _userRepository;

        private readonly UserService _service;

        public UserServiceTests()
        {
            _userRepository = new Mock<IUserRepository>();

            _service = new UserService(_userRepository.Object);
        }

        [Fact]
        public async Task GivenGetByCpfRequest_WhenUserDoesNotExists_ThenShouldThrowNotFoundException()
        {
            // arrange
            var cpf = "123.456.789-10";

            // act
            Func<Task> task = async () => await _service.GetByCpfAsync(cpf);

            // assert
            await task.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage("Usuário não encontrado");
        }

        [Fact]
        public async Task GivenGetByCpfRequest_WhenUserExists_ThenShouldReturnUser()
        {
            // arrange
            var cpf = "123.456.789-10";

            var mockedUser = UserMock.Get(cpf: cpf);

            _userRepository
                .Setup(r => r.FindFirstDefaultAsync(
                    x => x.Cpf == "12345678910"
                ))
                .ReturnsAsync(mockedUser);

            // act
            var response = await _service.GetByCpfAsync(cpf);

            // assert
            response.Should().BeEquivalentTo(mockedUser);;
        }

        [Fact]
        public async Task GivenGetByEmailRequest_WhenUserDoesNotExists_ThenShouldThrowNotFoundException()
        {
            // arrange
            var email = "test@email.com";

            // act
            Func<Task> task = async () => await _service.GetByEmailAsync(email);

            // assert
            await task.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage("Usuário não encontrado");
        }

        [Fact]
        public async Task GivenGetByEmailRequest_WhenUserExists_ThenShouldReturnUser()
        {
            // arrange
            var email = "test@email.com";

            var mockedUser = UserMock.Get(email: email);

            _userRepository
                .Setup(r => r.FindFirstDefaultAsync(
                    x => x.Email == email.Trim()
                ))
                .ReturnsAsync(mockedUser);

            // act
            var response = await _service.GetByEmailAsync(email);

            // assert
            response.Should().BeEquivalentTo(mockedUser); ;
        }

        [Fact]
        public async Task GivenCreateRequest_WhenUserAlreadyExists_ThenShouldThrowDomainException()
        {
            // arrange
            var request = new UserCreateRequestDTO
            (
                "123.456.789-10",
                "Name",
                "email",
                "password",
                UserRole.Customer
            );

            _userRepository
                .Setup(r => r.AnyAsync(x => x.Cpf == "12345678910"))
                .ReturnsAsync(true);

            // act
            Func<Task> task = async () => await _service.CreateAsync(request);

            // assert
            await task.Should()
                .ThrowAsync<DomainException>()
                .WithMessage("CPF já cadastrado");
        }

        [Fact]
        public async Task GivenCreateRequest_WhenUserDoesNotExist_ThenShouldCreateUser()
        {
            // arrange
            var request = new UserCreateRequestDTO
            (
                "123.456.789-10",
                "Test Name",
                "test@email.com",
                "password",
                UserRole.Customer
            );

            // act
            var response = await _service.CreateAsync(request);

            // assert
            _userRepository
                .Verify(
                    x => x.AddAsync(It.IsAny<User>()),
                    Times.Once
                );

            response.Should().BeOfType<UserDTO>();
            response.Cpf.Should().Be(request.Cpf);
        }

        [Fact]
        public async Task GivenUpdateRequest_WhenUserDoesNotExist_ThenShouldThrowNotFoundException()
        {
            // arrange
            var userId = Guid.NewGuid();
            var request = new UserUpdateRequestDTO
            (
                "Name",
                "email",
                "password",
                UserRole.Customer
            );

            // act
            Func<Task> task = async () => await _service.UpdateAsync(userId, request);

            // assert
            await task.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage("Usuário não encontrado");
        }

        [Fact]
        public async Task GivenUpdateRequest_WhenUserExists_ThenShouldUpdateUser()
        {
            // arrange
            var request = new UserUpdateRequestDTO
            (
                "Updated Name",
                "updated@email.com",
                "password",
                UserRole.Customer
            );

            var mockedUser = UserMock.Get(
                "Initial Name",
                "old@email.com",
                userRole: UserRole.Customer
            );

            _userRepository
                .Setup(r => r.GetByIdAsync(
                    It.IsAny<Guid>()
                ))
                .ReturnsAsync(mockedUser);

            // act
            var response = await _service.UpdateAsync(mockedUser.Id, request);

            // assert
            _userRepository
                .Verify(
                    x => x.UpdateAsync(It.IsAny<User>()),
                    Times.Once
                );

            response.Should().BeOfType<UserDTO>();

            response.Name.Should().Be(request.Name);
            response.Email.Should().Be(request.Email);
            response.UserType.Should().Be(request.UserType);
        }

        [Fact]
        public async Task GivenDeleteRequest_WhenUserDoesNotExist_ThenShouldThrowNotFoundException()
        {
            // arrange
            var userId = Guid.NewGuid();

            // act
            Func<Task> task = async () => await _service.DeleteAsync(userId);

            // assert
            await task.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage("Usuário não encontrado");
        }

        [Fact]
        public async Task GivenDeleteRequest_WhenUserExists_ThenShouldRemoveUser()
        {
            // arrange
            var mockedUser = UserMock.Get();

            _userRepository
                .Setup(r => r.FindFirstDefaultAsync(
                    It.IsAny<Expression<Func<User, bool>>>()
                ))
                .ReturnsAsync(mockedUser);

            // act
            await _service.DeleteAsync(mockedUser.Id);

            // assert
            _userRepository
                .Verify(
                    x => x.Remove(It.IsAny<User>()),
                    Times.Once
                );
        }

        [Fact]
        public async Task GivenGetDtoByIdRequest_WhenUserDoesNotExist_ThenShouldThrowNotFoundException()
        {
            // arrange
            var userId = Guid.NewGuid();

            // act
            Func<Task> task = async () => await _service.GetDtoByIdAsync(userId);

            // assert
            await task.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage("Usuário não encontrado");
        }

        [Fact]
        public async Task GivenGetDtoByIdRequest_WhenUserExists_ThenShouldReturnUserDto()
        {
            // arrange
            var mockedUser = UserMock.Get();

            _userRepository
                .Setup(r => r.FindFirstDefaultAsync(
                    It.IsAny<Expression<Func<User, bool>>>()
                ))
                .ReturnsAsync(mockedUser);

            // act
            var response = await _service.GetDtoByIdAsync(mockedUser.Id);

            // assert
            response.Should().BeOfType<UserDTO>();

            response.Cpf.Should().Be(Format.FormatCpf(mockedUser.Cpf));
            response.Name.Should().Be(mockedUser.Name);
            response.Email.Should().Be(mockedUser.Email);
            response.UserType.Should().Be(mockedUser.UserRole);
        }

        [Fact]
        public async Task GivenGetUsersRequest_WhenNoUserTypeIsInformed_ThenShouldGetAllUsersAndReturnList()
        {
            // arrange
            var mockedUsers = UserMock.GetList(3);

            _userRepository
                .Setup(r => r.GetAllAsync())
                .ReturnsAsync(mockedUsers);

            // act
            var response = await _service.GetUsersDtoAsync(null);

            // assert
            response.Should().BeAssignableTo<IEnumerable<UserDTO>>();
            response.Should().HaveCount(3);

            _userRepository.Verify(
                x => x.GetAllAsync(),
                Times.Once
            );

            _userRepository.Verify(
                x => x.FindAsync(It.IsAny<Expression<Func<User, bool>>>()),
                Times.Never
            );
        }

        [Theory]
        [InlineData(UserRole.Customer)]
        [InlineData(UserRole.Employee)]
        public async Task GivenGetUsersDtoRequest_WhenUserTypeIsInformed_ThenShouldGetUsersByUserTypeAndReturnList
        (
            UserRole userType
        )
        {
            // arrange
            var mockedUsers = UserMock.GetList(3, userType);

            _userRepository
                .Setup(r => r.FindAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(mockedUsers);

            // act
            var response = await _service.GetUsersDtoAsync(userType);

            // assert
            response.Should().BeAssignableTo<IEnumerable<UserDTO>>();
            response.Should().HaveCount(3);
            response.Should().OnlyContain(x => x.UserType == userType);

            _userRepository.Verify(
                x => x.FindAsync(It.IsAny<Expression<Func<User, bool>>>()),
                Times.Once
            );

            _userRepository.Verify(
                x => x.GetAllAsync(),
                Times.Never
            );
        }
    }
}
