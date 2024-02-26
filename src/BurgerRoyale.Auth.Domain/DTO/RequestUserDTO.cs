using BurgerRoyale.Auth.Domain.Enumerators;

namespace BurgerRoyale.Auth.Domain.DTO
{
    public class RequestUserDTO
    {
        public string Cpf { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public UserType UserType { get; set; }
    }
}
