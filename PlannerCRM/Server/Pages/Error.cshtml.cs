namespace PlannerCRM.Server.Pages;

[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
[IgnoreAntiforgeryToken]
public class ErrorModel : PageModel
{
    public string RequestId { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

    private readonly ILogger<ErrorModel> _logger;

    public ErrorModel(ILogger<ErrorModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
        RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier;
        var exceptionHandlerPathFeature =
            HttpContext.Features.Get<IExceptionHandlerPathFeature>();

        if (exceptionHandlerPathFeature.Error is NotSupportedException)
        {
            _logger.Log(LogLevel.Error, "Richiesta http non valida.");
        }
        else if (exceptionHandlerPathFeature.Error is InvalidOperationException)
        {
            _logger.Log(LogLevel.Error, "Operazione non valida.");
        }
        else if (exceptionHandlerPathFeature.Error is ArgumentNullException)
        {
            _logger.Log(LogLevel.Error, "Argomenti di tipo null non validi.");
        }
        else
        {
            _logger.LogWarning("Qualcosa è andato storto.");
        }
    }
}
