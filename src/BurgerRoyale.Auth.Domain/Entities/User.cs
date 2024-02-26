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
        public virtual UserType UserType { get; private set; }

        public User(string cpf, string name, string email, string passwordHash, UserType userType)
        {
            Cpf = Format.NormalizeCpf(cpf);
            Name = name;
            Email = email;
            PasswordHash = passwordHash;
            UserType = userType;
        }
    }
}
