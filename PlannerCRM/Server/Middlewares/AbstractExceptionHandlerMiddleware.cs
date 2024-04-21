using System.Net;
using System.Reflection;
using Serilog;

namespace PlannerCRM.Server.Middlewares;

public abstract class AbstractExceptionHandlerMiddleware : IMiddleware
{
    private static readonly Serilog.ILogger Logger = Log.ForContext(MethodBase.GetCurrentMethod()?.DeclaringType);

    public abstract (HttpStatusCode code, string message) GetResponse(Exception exception);

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception exc)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            Logger.Error(context.Request.Method, $"Error during executing {context}", context.Request.Path.Value);

            var (status, message) = GetResponse(exc);
            response.StatusCode = (int)status;
            await response.WriteAsync(message);
        }
    }
}