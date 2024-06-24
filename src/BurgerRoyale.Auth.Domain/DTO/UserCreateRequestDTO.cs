using BurgerRoyale.Auth.Domain.Enumerators;

namespace BurgerRoyale.Auth.Domain.DTO
{
    public record UserCreateRequestDTO
    (
        string Cpf,
        string Name,
        string Email,
        string? Phone,
        string? Address,
        string Password,
        UserRole UserRole
    );
}
