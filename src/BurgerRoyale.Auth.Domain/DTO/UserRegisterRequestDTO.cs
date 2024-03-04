namespace BurgerRoyale.Auth.Domain.DTO
{
    public record UserRegisterRequestDTO
    (
        string Cpf,
        string Name,
        string Email,
        string Password,
        string PasswordConfirmation
    );
}
