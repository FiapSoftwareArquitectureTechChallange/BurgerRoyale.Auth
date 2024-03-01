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
        public string PasswordHash { get; private set; }
        public virtual UserRole UserRole { get; private set; }

        public User(string cpf, string name, string email, string passwordHash, UserRole userRole)
        {
            Cpf = Format.NormalizeCpf(cpf);
            Name = name;
            Email = email;
            PasswordHash = passwordHash;
            UserRole = userRole;

            ValidateEntity();
        }

        public void SetDetails(string name, string email, string passwordHash, UserRole userRole)
        {
            Name = name;
            Email = email;
            PasswordHash = passwordHash;
            UserRole = userRole;

            ValidateEntity();
        }

        private void ValidateEntity()
        {
            new UserValidator().Validate(this);
        }
    }
}
