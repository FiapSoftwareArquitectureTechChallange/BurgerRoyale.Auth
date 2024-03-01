using BurgerRoyale.Auth.Domain.Enumerators;
using BurgerRoyale.Auth.Domain.Helpers;

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
        }

        public void SetDetails(string name, string email, string passwordHash, UserRole userRole)
        {
            Name = name;
            Email = email;
            PasswordHash = passwordHash;
            UserRole = userRole;
        }
    }
}
