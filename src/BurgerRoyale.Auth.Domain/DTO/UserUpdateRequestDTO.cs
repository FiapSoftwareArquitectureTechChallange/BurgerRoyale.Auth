using BurgerRoyale.Auth.Domain.Enumerators;

namespace BurgerRoyale.Auth.Domain.DTO
{
    public record UserUpdateRequestDTO
    (
        string Name,
        string Email,
        string? Phone,
        string? Address,
        string Password,
        UserRole UserRole
    );
}
