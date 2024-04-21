
using System.Net;

namespace PlannerCRM.Server.Middlewares;

public abstract class ExceptionHandlerMiddleware : IMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;

    public ExceptionHandlerMiddleware(RequestDelegate next, ILogger logger)
    {
        _next = next;
        _logger = logger;
    }

    public abstract (HttpStatusCode code, string message) GetResponse(Exception exception);

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exc)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            _logger.LogError(context.Request.Method, $"Error during executing {context}", context.Request.Path.Value);

            var (status, message) = GetResponse(exc);
            response.StatusCode = (int)status;
            await response.WriteAsync(message);
        }
    }
}