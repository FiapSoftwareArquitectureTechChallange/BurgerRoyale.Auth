using BurgerRoyale.Auth.Domain.Configurations;
using BurgerRoyale.Auth.Domain.DTO;
using BurgerRoyale.Auth.Domain.Entities;
using BurgerRoyale.Auth.Domain.Helpers;
using BurgerRoyale.Auth.Domain.Interface.Repositories;
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
        private readonly IUserRepository _userRepository;
        private readonly JwtConfiguration _jwtConfiguration;

        public AccountService
        (
            IUserRepository userRepository,
            IOptions<JwtConfiguration> jwtConfiguration
        )
        {
            _userRepository = userRepository;
            _jwtConfiguration = jwtConfiguration.Value;
        }

        public async Task<AuthenticationResponseDTO> Authenticate(AuthenticationRequestDTO request)
        {
            try
            {
                User user = await _userRepository.FindFirstDefaultAsync(x =>
                    x.Cpf == request.Cpf || x.Email == request.Email
                );

                if ( user == null || !BC.Verify(request.Password, user.PasswordHash))
                {
                    throw new UnauthorizedAccessException("Credenciais incorretas");
                }

                UserDTO userDto = new UserDTO(user);
                string token = GenerateJwtToken(user);

                return new AuthenticationResponseDTO(userDto, token);
            }
            catch (Exception)
            {
                throw new UnauthorizedAccessException("Usuário não autorizado");
            }
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
