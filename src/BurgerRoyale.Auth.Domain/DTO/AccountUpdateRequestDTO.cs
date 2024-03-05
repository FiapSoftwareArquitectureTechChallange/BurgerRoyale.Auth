namespace BurgerRoyale.Auth.Domain.DTO
{
    public record AccountUpdateRequestDTO
    (
        string Name,
        string Email,
        string CurrentPassword,
        string NewPassword,
        string NewPasswordConfirmation
    );
}
