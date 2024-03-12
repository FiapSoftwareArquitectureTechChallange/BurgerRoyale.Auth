using BurgerRoyale.Auth.Domain.Enumerators;

namespace BurgerRoyale.Auth.Domain.DTO
{
    public record UserCreateRequestDTO
    (
        string Cpf,
        string Name,
        string Email,
        string Password,
        UserRole UserRole
    );
}
