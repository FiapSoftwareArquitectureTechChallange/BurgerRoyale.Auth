namespace BurgerRoyale.Auth.Domain.DTO
{
    public record CustomerRequestDTO
    (
        string Cpf,
        string Name,
        string Email,
        string Password,
        string PasswordConfirmation
    );
}
