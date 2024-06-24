using BurgerRoyale.Auth.Domain.DTO;
using BurgerRoyale.Auth.Domain.Enumerators;
using BurgerRoyale.Auth.Domain.Helpers;
using BurgerRoyale.Auth.Domain.Validators;

namespace BurgerRoyale.Auth.Domain.Entities
{
    public class User : Entity
    {
        public string Cpf { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string? Phone { get; private set; }
        public string? Address { get; private set; }
        public string PasswordHash { get; private set; }
        public virtual UserRole UserRole { get; private set; }

        public User
        (
            string cpf,
            string name,
            string email,
            string? phone,
            string? address,
            string passwordHash,
            UserRole userRole
        )
        {
            Cpf = Format.NormalizeCpf(cpf);
            Name = name;
            Email = email;
            Phone = phone;
            Address = address;
            PasswordHash = passwordHash;
            UserRole = userRole;

            ValidateEntity();
        }

        public void SetDetails
        (
            string name,
            string email,
            string? phone,
            string? address,
            string passwordHash,
            UserRole userRole
        )
        {
            Name = name;
            Email = email;
            Phone = phone;
            Address = address;
            PasswordHash = passwordHash;
            UserRole = userRole;

            ValidateEntity();
        }

        public UserDTO AsDto()
        {
            return new UserDTO(this);
        }

        private void ValidateEntity()
        {
            new UserValidator().Validate(this);
        }
    }
}
