using BurgerRoyale.Auth.Domain.DTO;
using BurgerRoyale.Auth.Domain.Enumerators;

namespace BurgerRoyale.Auth.Domain.Interface.Services
{
    public interface IUserService
    {
        Task<UserDTO> GetDtoByIdAsync(Guid userId);

        Task<IEnumerable<UserDTO>> GetUsersDtoAsync(UserRole? userType);

        Task<UserDTO> CreateAsync(RequestUserDTO model);

        Task DeleteAsync(Guid userId);

        Task<UserDTO> GetByCpfAsync(string cpf);

        Task<UserDTO> UpdateAsync(Guid userId, RequestUserDTO model);
    }
}
