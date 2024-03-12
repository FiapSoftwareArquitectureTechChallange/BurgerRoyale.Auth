namespace BurgerRoyale.Auth.API.Middleware;

/// <summary>
/// Error response
/// </summary>
/// <param name="ErrorMessage">Error message</param>
/// <param name="TraceId">Request TraceId</param>
public record ErrorResponse
(
    string? ErrorMessage,

    string? TraceId
);
