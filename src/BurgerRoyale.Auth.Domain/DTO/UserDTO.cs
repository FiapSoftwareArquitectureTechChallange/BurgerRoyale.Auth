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
        public UserRole UserType { get; set; }

        public string UserTypeDescription
        {
            get => UserType.GetDescription();
        }

        public UserDTO(User user)
        {
            Id = user.Id;
            Cpf = Format.FormatCpf(user.Cpf);
            Name = user.Name;
            Email = user.Email;
            UserType = user.UserRole;
        }
    }
}
