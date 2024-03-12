namespace BurgerRoyale.Auth.Domain.DTO
{
    public record AuthenticationResponseDTO
    (
        UserDTO User,
        string AccessToken
    );
}
