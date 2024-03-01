using BurgerRoyale.Auth.Domain.DTO;
using BurgerRoyale.Auth.Domain.Entities;
using BurgerRoyale.Auth.Domain.Interface.Repositories;
using BurgerRoyale.Auth.Domain.Interface.Services;
using BC = BCrypt.Net.BCrypt;

namespace BurgerRoyale.Auth.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUserRepository _userRepository;

        public AccountService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
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
            }
            catch (Exception)
            {
                throw new UnauthorizedAccessException("Usuário não autorizado");
            }

            throw new NotImplementedException();
        }
    }
}
