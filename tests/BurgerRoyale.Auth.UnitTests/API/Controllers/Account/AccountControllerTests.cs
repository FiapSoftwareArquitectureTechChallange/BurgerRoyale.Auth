using BurgerRoyale.Auth.API.Controllers.Account;
using BurgerRoyale.Auth.Domain.DTO;
using BurgerRoyale.Auth.Domain.Interface.Services;
using BurgerRoyale.Auth.UnitTests.Domain.EntitiesMocks;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BurgerRoyale.Auth.UnitTests.API.Controllers.Account
{
    public class AccountControllerTests
    {
        private readonly Mock<IAccountService> _accountServiceMock;
        
        private readonly Mock<IAuthenticatedUser> _authenticatedUserMock;
        
        private readonly AccountController _accountController;

        public AccountControllerTests()
        {
            _accountServiceMock = new Mock<IAccountService>();

            _authenticatedUserMock = new Mock<IAuthenticatedUser>();

            _accountController = new AccountController(
                _accountServiceMock.Object,
                _authenticatedUserMock.Object);
        }

        [Fact]
        public async Task GivenAuthenticationRequest_WhenAuthenticate_ThenShouldReturnResponseWithAccessToken()
        {
            // arrange
            var userDto = UserMock.Get().AsDto();
            var accessToken = "access_token";

            var authenticationRequest = new AuthenticationRequestDTO(null, "email@test.com", "password");

            _accountServiceMock
                .Setup(x => x.Authenticate(authenticationRequest))
                .ReturnsAsync(new AuthenticationResponseDTO(
                    userDto,
                    accessToken
                ));

            // act
            var response = await _accountController.Authenticate(authenticationRequest) as ObjectResult;

            // assert
            response?.StatusCode.Should().Be((int)HttpStatusCode.OK);
            response?.Value.Should().BeAssignableTo<AuthenticationResponseDTO>();

            (response?.Value as AuthenticationResponseDTO)?.AccessToken.Should().Be(accessToken);
        }

        [Fact]
        public async Task GivenCustomerRequest_WhenRegisterCustomer_ThenShouldReturnCreatedUserDto()
        {
            // arrange
            var password = "password";

            var userDto = UserMock.Get(password: password).AsDto();

            var customerRequest = new CustomerRequestDTO(
                userDto.Cpf,
                userDto.Name,
                userDto.Email,
                userDto.Phone,
                userDto.Address,
                password,
                password
            );

            _accountServiceMock
                .Setup(x => x.RegisterCustomerAsync(customerRequest))
                .ReturnsAsync(userDto);

            // act
            var response = await _accountController.RegisterCustomer(customerRequest) as ObjectResult;

            // assert
            response?.StatusCode.Should().Be((int)HttpStatusCode.Created);
            response?.Value.Should().BeEquivalentTo(userDto);
        }

        [Fact]
        public async Task GivenAccountUpdateRequestDTO_WhenUpdateAccount_ThenShouldReturnUpdatedUserDto()
        {
            // arrange
            var password = "password";

            var userDto = UserMock.Get(password: password).AsDto();

            var updateRequest = new AccountUpdateRequestDTO(
                userDto.Name,
                userDto.Email,
                userDto.Phone,
                userDto.Address,
                password,
                password,
                password
            );

            _accountServiceMock
                .Setup(x => x.UpdateAccountAsync(userDto.Id, updateRequest))
                .ReturnsAsync(userDto);

            // act
            var response = await _accountController.UpdateAccount(userDto.Id, updateRequest) as ObjectResult;

            // assert
            response?.StatusCode.Should().Be((int)HttpStatusCode.OK);
            response?.Value.Should().BeEquivalentTo(userDto);
        }
        
        [Fact]
        public async Task GivenAccountUnregisterRequest_WhenUnregisterAccount_ThenShouldReturnUpdatedUserDto()
        {
            // arrange
            var userId = Guid.NewGuid();

            _authenticatedUserMock
                .Setup(x => x.Id)
                .Returns(userId);   

            // act
            var response = await _accountController.UnregisterAccount() as ObjectResult;

            // assert
            response?.StatusCode.Should().Be((int)HttpStatusCode.OK);

            _accountServiceMock
                .Verify(x => x.UnregisterAsync(userId), 
                Times.Once);
        }
    }
}