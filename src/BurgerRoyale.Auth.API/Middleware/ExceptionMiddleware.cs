using BurgerRoyale.Auth.Domain.Exceptions;
using System.Net;
using System.Text.Json;

namespace BurgerRoyale.Auth.API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception exception)
            {
                await HandleExceptionAsync(context, exception);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json; charset=utf-8";

            var statusCode = MapHttpStatusCode(exception);

            context.Response.StatusCode = (int)statusCode;

            var response = GetErrorResponse(context, exception);

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }

        private ErrorResponse GetErrorResponse(HttpContext context, Exception exception)
        {
            return new ErrorResponse(
                exception.Message,
                context.TraceIdentifier
            );
        }

        private HttpStatusCode MapHttpStatusCode(Exception exception) => exception switch
        {
            var e when e is DomainException => HttpStatusCode.BadRequest,
            var e when e is NotFoundException => HttpStatusCode.NotFound,
            var e when e is UnauthorizedAccessException => HttpStatusCode.Unauthorized,
            _ => HttpStatusCode.InternalServerError
        };
    }
}
