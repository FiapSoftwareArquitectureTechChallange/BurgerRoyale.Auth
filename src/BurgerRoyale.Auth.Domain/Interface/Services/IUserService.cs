using BurgerRoyale.Auth.Domain.DTO;
using BurgerRoyale.Auth.Domain.Entities;
using BurgerRoyale.Auth.Domain.Enumerators;

namespace BurgerRoyale.Auth.Domain.Interface.Services
{
    public interface IUserService
    {
        Task<UserDTO> GetDtoByIdAsync(Guid userId);

        Task<IEnumerable<UserDTO>> GetUsersDtoAsync(UserRole? userType);

        Task<UserDTO> CreateAsync(UserCreateRequestDTO model);

        Task DeleteAsync(Guid userId);

        Task<User> GetByCpfAsync(string cpf);

        Task<User> GetByEmailAsync(string email);

        Task<UserDTO> UpdateAsync(Guid userId, UserUpdateRequestDTO model);
    }
}
