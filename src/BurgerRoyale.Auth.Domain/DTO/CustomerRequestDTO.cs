namespace BurgerRoyale.Auth.Domain.DTO
{
    public record CustomerRequestDTO
    (
        string Cpf,
        string Name,
        string Email,
        string? Phone,
        string? Address,
        string Password,
        string PasswordConfirmation
    );
}
