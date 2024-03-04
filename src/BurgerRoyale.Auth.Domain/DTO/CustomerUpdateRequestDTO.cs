namespace BurgerRoyale.Auth.Domain.DTO
{
    public record CustomerUpdateRequestDTO
    (
        string Name,
        string Email,
        string CurrentPassword,
        string NewPassword,
        string NewPasswordConfirmation
    );
}
