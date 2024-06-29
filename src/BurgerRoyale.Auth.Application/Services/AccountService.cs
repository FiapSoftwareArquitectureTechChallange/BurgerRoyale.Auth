using BurgerRoyale.Auth.Domain.Configurations;
using BurgerRoyale.Auth.Domain.DTO;
using BurgerRoyale.Auth.Domain.Entities;
using BurgerRoyale.Auth.Domain.Enumerators;
using BurgerRoyale.Auth.Domain.Exceptions;
using BurgerRoyale.Auth.Domain.Helpers;
using BurgerRoyale.Auth.Domain.Interface.Services;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BC = BCrypt.Net.BCrypt;

namespace BurgerRoyale.Auth.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUserService _userService;

        private readonly JwtConfiguration _jwtConfiguration;

        public AccountService(
            IUserService userService,
            IOptions<JwtConfiguration> jwtConfiguration)
        {
            _userService = userService;
            _jwtConfiguration = jwtConfiguration.Value;
        }

        public async Task<AuthenticationResponseDTO> Authenticate(AuthenticationRequestDTO request)
        {
            try
            {
                User user = await GetUser(request);

                ValidateUserLogin(request, user);

                UserDTO userDto = user.AsDto();

                string token = GenerateJwtToken(user);

                return new AuthenticationResponseDTO(userDto, token);
            }
            catch (Exception)
            {
                throw new UnauthorizedAccessException("User not authorized");
            }
        }

        private async Task<User> GetUser(AuthenticationRequestDTO request)
        {
            if (!string.IsNullOrWhiteSpace(request.Cpf))
                return await _userService.GetByCpfAsync(request.Cpf);

            return await _userService.GetByEmailAsync(request.Email ?? string.Empty);
        }

        private static void ValidateUserLogin(AuthenticationRequestDTO request, User user)
        {
            if (!BC.Verify(request.Password, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Incorrect credentials");
            }
        }

        public async Task<UserDTO> RegisterCustomerAsync(CustomerRequestDTO request)
        {
            EnsurePasswordsMatch(request.Password, request.PasswordConfirmation);

            return await RegisterCustomer(request);
        }

        private static void EnsurePasswordsMatch(string password, string passwordConfirmation)
        {
            if (password != passwordConfirmation)
            {
                throw new DomainException("Wrong passwords");
            }
        }

        private async Task<UserDTO> RegisterCustomer(CustomerRequestDTO request)
        {
            UserCreateRequestDTO user = CreateUserDTO(request);

            return await _userService.CreateAsync(user);
        }

        private static UserCreateRequestDTO CreateUserDTO(CustomerRequestDTO request)
        {
            return new UserCreateRequestDTO(
                request.Cpf,
                request.Name,
                request.Email,
                request.Phone,
                request.Address,
                request.Password,
                UserRole.Customer
            );
        }

        public async Task UnregisterAsync(Guid userId)
        {
            await _userService.DeleteAsync(userId);
        }

        public async Task<UserDTO> UpdateAccountAsync(Guid userId, AccountUpdateRequestDTO request)
        {
            EnsurePasswordsMatch(request.NewPassword, request.NewPasswordConfirmation);

            var user = await _userService.GetByIdAsync(userId);

            if (!BC.Verify(request.CurrentPassword, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Current password incorrect");
            }

            UserUpdateRequestDTO updateUserDTO = CreateUserUpdateRequest(request, user);

            return await _userService.UpdateAsync(userId, updateUserDTO);
        }

        private static UserUpdateRequestDTO CreateUserUpdateRequest(AccountUpdateRequestDTO request, User user)
        {
            return new UserUpdateRequestDTO(
                    request.Name,
                    request.Email,
                    request.Phone,
                    request.Address,
                    request.NewPassword,
                    user.UserRole
            );
        }

        private string GenerateJwtToken(User user)
        {
            SymmetricSecurityKey securityKey = CreateSecurityKey();

            SigningCredentials credentials = CreateCredentials(securityKey);

            Claim[] claims = CreateClaims(user);

            JwtSecurityToken securityToken = CreateSecurityToken(credentials, claims);

            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }

        private SymmetricSecurityKey CreateSecurityKey()
        {
            return new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_jwtConfiguration.SecretKey!)
            );
        }

        private static SigningCredentials CreateCredentials(SymmetricSecurityKey securityKey)
        {
            return new SigningCredentials(
                securityKey,
                SecurityAlgorithms.HmacSha256
            );
        }

        private static Claim[] CreateClaims(User user)
        {
            return [
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.UserRole.GetDescription())
            ];
        }

        private JwtSecurityToken CreateSecurityToken(SigningCredentials credentials, Claim[] claims)
        {
            return new JwtSecurityToken
            (
                _jwtConfiguration.Issuer,
                _jwtConfiguration.Audience,
                claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: credentials
            );
        }
    }
}