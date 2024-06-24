using BurgerRoyale.Auth.Domain.DTO;
using BurgerRoyale.Auth.Domain.Entities;
using BurgerRoyale.Auth.Domain.Enumerators;
using BurgerRoyale.Auth.Domain.Exceptions;
using BurgerRoyale.Auth.Domain.Helpers;
using BurgerRoyale.Auth.Domain.Interface.Repositories;
using BurgerRoyale.Auth.Domain.Interface.Services;
using BC = BCrypt.Net.BCrypt;

namespace BurgerRoyale.Auth.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }


        public async Task<User> GetByCpfAsync(string cpf)
        {
            cpf = Format.NormalizeCpf(cpf);

            User? user = await _userRepository.FindFirstDefaultAsync(x => x.Cpf == cpf);

            if (user is null)
            {
                throw new NotFoundException("User not found");
            }

            return user;
        }

        public async Task<UserDTO> CreateAsync(UserCreateRequestDTO model)
        {
            var cpf = Format.NormalizeCpf(model.Cpf);

            bool userAlreadyExists = await _userRepository.AnyAsync(x => x.Cpf == cpf);

            if (userAlreadyExists)
            {
                throw new DomainException("CPF already registered");
            }

            var user = new User(
                cpf,
                model.Name,
                model.Email,
                model.Phone,
                model.Address,
                BC.HashPassword(model.Password),
                model.UserRole
            );

            await _userRepository.AddAsync(user);

            return user.AsDto();
        }

        public async Task<UserDTO> UpdateAsync(Guid userId, UserUpdateRequestDTO model)
        {
            User? user = await _userRepository.GetByIdAsync(userId);

            if (user is null)
            {
                throw new NotFoundException("User not found");
            }

            user.SetDetails(
                model.Name,
                model.Email,
                model.Phone,
                model.Address,
                BC.HashPassword(model.Password),
                model.UserRole
            );

            await _userRepository.UpdateAsync(user);

            return user.AsDto();
        }

        public async Task DeleteAsync(Guid userId)
        {
            User? user = await _userRepository.FindFirstDefaultAsync(x => x.Id == userId);

            if (user is null)
            {
                throw new NotFoundException("User not found");
            }

            _userRepository.Remove(user);
        }

        public async Task<User> GetByIdAsync(Guid userId)
        {
            User? user = await _userRepository.FindFirstDefaultAsync(x => x.Id == userId);

            if (user is null)
            {
                throw new NotFoundException("User not found");
            }

            return user;
        }

        public async Task<UserDTO> GetDtoByIdAsync(Guid userId)
        {
            User user = await GetByIdAsync(userId);

            return user.AsDto();
        }

        public async Task<IEnumerable<UserDTO>> GetUsersDtoAsync(UserRole? userRole)
        {
            var users = (userRole == null)
                ? await _userRepository.GetAllAsync()
                : await _userRepository.FindAsync(x => x.UserRole == userRole);

            return users.Select(user => user.AsDto());
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            User? user = await _userRepository.FindFirstDefaultAsync(x => x.Email == email.Trim());

            if (user is null)
            {
                throw new NotFoundException("User not found");
            }

            return user;
        }
    }
}
