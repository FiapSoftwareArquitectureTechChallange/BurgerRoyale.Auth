using BurgerRoyale.Auth.Domain.DTO;

namespace BurgerRoyale.Auth.Domain.Interface.Services
{
    public interface IAccountService
    {
        Task<AuthenticationResponseDTO> Authenticate(AuthenticationRequestDTO request);

        Task<UserDTO> RegisterCustomer (UserRegisterRequestDTO request);
    }
}
