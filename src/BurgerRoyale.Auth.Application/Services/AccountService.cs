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

        public AccountService
        (
            IUserService userService,
            IOptions<JwtConfiguration> jwtConfiguration
        )
        {
            _userService = userService;
            _jwtConfiguration = jwtConfiguration.Value;
        }

        public async Task<AuthenticationResponseDTO> Authenticate(AuthenticationRequestDTO request)
        {
            try
            {
                User user = !string.IsNullOrWhiteSpace(request.Cpf)
                    ? await _userService.GetByCpfAsync(request.Cpf)
                    : await _userService.GetByEmailAsync(request.Email ?? string.Empty);

                if (!BC.Verify(request.Password, user.PasswordHash))
                {
                    throw new UnauthorizedAccessException("Incorrect credentials");
                }

                UserDTO userDto = user.AsDto();
                string token = GenerateJwtToken(user);

                return new AuthenticationResponseDTO(userDto, token);
            }
            catch (Exception)
            {
                throw new UnauthorizedAccessException("User not authorized");
            }
        }

        public async Task<UserDTO> RegisterCustomerAsync(CustomerRequestDTO request)
        {
            if (request.Password != request.PasswordConfirmation)
            {
                throw new DomainException("Wrong passwords");
            }

            return await _userService.CreateAsync(
                new UserCreateRequestDTO(
                    request.Cpf,
                    request.Name,
                    request.Email,
                    request.Phone,
                    request.Address,
                    request.Password,
                    UserRole.Customer
                )
            );
        }

        public async Task UnregisterAsync(Guid userId)
        {
            await _userService.DeleteAsync(userId);
        }

        public async Task<UserDTO> UpdateAccountAsync(Guid userId, AccountUpdateRequestDTO request)
        {
            if (request.NewPassword != request.NewPasswordConfirmation)
            {
                throw new DomainException("Wrong passwords");
            }

            var user = await _userService.GetByIdAsync(userId);

            if (!BC.Verify(request.CurrentPassword, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Current password incorrect");
            }

            return await _userService.UpdateAsync(
                userId,
                new UserUpdateRequestDTO(
                    request.Name,
                    request.Email,
                    request.Phone,
                    request.Address,
                    request.NewPassword,
                    user.UserRole
                )
            );
        }

        private string GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey
            (
                Encoding.UTF8.GetBytes(_jwtConfiguration.SecretKey!)
            );

            var credentials = new SigningCredentials
            (
                securityKey,
                SecurityAlgorithms.HmacSha256
            );

            var claims = new []
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.UserRole.GetDescription())
            };

            var securityToken = new JwtSecurityToken
            (
                _jwtConfiguration.Issuer,
                _jwtConfiguration.Audience,
                claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }
    }
}
