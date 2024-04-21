namespace PlannerCRM.Server.Controllers;

[Authorize(Roles = nameof(Roles.PROJECT_MANAGER))]
[ApiController]
[Route("api/[controller]")]
public class CalculatorController : ControllerBase
{
    private readonly CalculatorService _calculator;
    private readonly ILogger<CalculatorService> _logger;

    public CalculatorController(
        CalculatorService calculator,
        ILogger<CalculatorService> logger)
    {
        _calculator = calculator;
        _logger = logger;
    }

    [HttpGet("get/paginated/{limit}/{offset}")]
    public async Task<List<WorkOrderViewDto>> GetPaginatedWorkOrderCostsAsync(int limit, int offset)
    {
        try
        {
            return await _calculator.GetPaginatedWorkOrdersCostsAsync(limit, offset);
        }
        catch (Exception exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return new();
        }
    }

    [HttpGet("generate/{workOrderId}")]
    public async Task<IActionResult> AddInvoiceAsync(string workOrderId)
    {
        try
        {
            await _calculator.AddInvoiceAsync(workOrderId);

            return Ok(SuccessfulCrudFeedBack.REPORT_CREATED);
        }
        catch (Exception exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return BadRequest();
        }
    }

    [HttpGet("get/invoice/{workOrderId}")]
    public async Task<WorkOrderCostDto> GetInvoiceAsync(string workOrderId)
    {
        try
        {
            return await _calculator.GetWorkOrderCostForViewByIdAsync(workOrderId);
        }
        catch (Exception exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return new();
        }
    }

    [HttpGet("get/size")]
    public async Task<int> GetCollectionSizeAsync()
    {
        try
        {
            return await _calculator.GetCollectionSizeAsync();
        }
        catch (Exception exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return new();
        }
    }
}