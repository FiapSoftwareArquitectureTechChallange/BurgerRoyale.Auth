using BurgerRoyale.Auth.Domain.Enumerators;

namespace BurgerRoyale.Auth.Domain.DTO
{
    public record RequestUserDTO
    (
        string Cpf,
        string Name,
        string Email,
        string Password,
        UserType UserType
    );
}
