using BurgerRoyale.Auth.Domain.Entities;
using BurgerRoyale.Auth.Domain.Enumerators;
using BurgerRoyale.Auth.Domain.Helpers;

namespace BurgerRoyale.Auth.Domain.DTO
{
    public class UserDTO
    {
        public Guid Id { get; set; }
        public string Cpf { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public UserRole UserRole { get; set; }

        public string UserRoleDescription
        {
            get => UserRole.GetDescription();
        }

        public UserDTO(User user)
        {
            Id = user.Id;
            Cpf = Format.FormatCpf(user.Cpf);
            Name = user.Name;
            Email = user.Email;
            Phone = user.Phone ?? string.Empty;
            Address = user.Address ?? string.Empty;
            UserRole = user.UserRole;
        }
    }
}
