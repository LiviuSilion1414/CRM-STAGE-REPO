using System.Text.Json;

namespace PlannerCRM.Server.Middlewares;

public class GlobalExceptionHandlerMiddleware : AbstractExceptionHandlerMiddleware
{
    public override (HttpStatusCode code, string message) GetResponse(Exception exception)
    {
        var code = exception switch
        {
            KeyNotFoundException => HttpStatusCode.NotFound,
            DuplicateElementException => HttpStatusCode.Conflict,
            UnauthorizedAccessException => HttpStatusCode.Unauthorized,
            ArgumentNullException => HttpStatusCode.BadRequest,
            _ => HttpStatusCode.InternalServerError,
        };

        return (code, JsonSerializer.Serialize(exception.Message));
    }
}