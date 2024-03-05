using BurgerRoyale.Auth.Domain.DTO;

namespace BurgerRoyale.Auth.Domain.Interface.Services
{
    public interface IAccountService
    {
        Task<AuthenticationResponseDTO> Authenticate(AuthenticationRequestDTO request);

        Task<UserDTO> RegisterCustomerAsync (CustomerRequestDTO request);

        Task<UserDTO> UpdateAccountAsync(Guid userId, AccountUpdateRequestDTO request);
    }
}
