namespace BurgerRoyale.Auth.Domain.DTO
{
    public record AuthenticationRequestDTO
    (
        string? Cpf,
        string? Email,
        string Password
    );
}
