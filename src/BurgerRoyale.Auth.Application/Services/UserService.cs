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

        public async Task<UserDTO> GetByCpfAsync(string cpf)
        {
            cpf = Format.NormalizeCpf(cpf);

            User? userEntity = await _userRepository.FindFirstDefaultAsync(x => x.Cpf == cpf);

            if (userEntity is null)
            {
                throw new NotFoundException("CPF não encontrado");
            }

            return new UserDTO(userEntity);
        }

        public async Task<UserDTO> CreateAsync(RequestUserDTO model)
        {
            var cpf = Format.NormalizeCpf(model.Cpf);

            bool userAlreadyExists = await _userRepository.AnyAsync(x => x.Cpf == cpf);

            if (userAlreadyExists)
            {
                throw new DomainException("CPF já cadastrado");
            }

            var user = new User(
                cpf,
                model.Email,
                model.Name,
                BC.HashPassword(model.Password),
                model.UserType
            );

            await _userRepository.AddAsync(user);

            return new UserDTO(user);
        }

        public async Task<UserDTO> UpdateAsync(Guid userId, RequestUserDTO model)
        {
            var cpf = Format.NormalizeCpf(model.Cpf);

            User? user = await _userRepository.FindFirstDefaultAsync(x =>
                x.Id == userId && x.Cpf == cpf
            );

            if (user is null)
            {
                throw new NotFoundException("Usuário não encontrado");
            }

            user.SetDetails(
                model.Name,
                model.Email,
                BC.HashPassword(model.Password),
                model.UserType
            );

            await _userRepository.UpdateAsync(user);

            return new UserDTO(user);
        }

        public async Task DeleteAsync(Guid userId)
        {
            User? user = await _userRepository.FindFirstDefaultAsync(x => x.Id == userId);

            if (user is null)
            {
                throw new NotFoundException("Usuário não encontrado");
            }

            _userRepository.Remove(user);
        }

        public async Task<UserDTO> GetByIdAsync(Guid userId)
        {
            User? user = await _userRepository.FindFirstDefaultAsync(x => x.Id == userId);

            if (user is null)
            {
                throw new NotFoundException("Usuário não encontrado");
            }

            return new UserDTO(user);
        }

        public async Task<IEnumerable<UserDTO>> GetUsersAsync(UserType? userType)
        {
            var users = (userType == null)
                ? await _userRepository.GetAllAsync()
                : await _userRepository.FindAsync(x => x.UserType == userType);

            return users.Select(user => new UserDTO(user));
        }
    }
}
