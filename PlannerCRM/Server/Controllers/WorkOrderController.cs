namespace PlannerCRM.Server.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class WorkOrderController : ControllerBase
{
    private readonly WorkOrderRepository _repo;
    private readonly ILogger<WorkOrderRepository> _logger;

    public WorkOrderController(
        WorkOrderRepository repo,
        Logger<WorkOrderRepository> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    [Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
    [HttpPost("add")]
    public async Task<IActionResult> AddWorkOrderAsync(WorkOrderFormDto dto)
    {
        try
        {
            await _repo.AddAsync(dto);

            return Ok(SuccessfulCrudFeedBack.WORKORDER_ADD);
        }
        catch (NullReferenceException exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return NotFound(exc.Message);
        }
        catch (ArgumentNullException exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return NotFound(exc.Message);
        }
        catch (DuplicateElementException exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return NotFound(exc.Message);
        }
        catch (DbUpdateException exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return StatusCode(StatusCodes.Status500InternalServerError);
        }
        catch (Exception exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return StatusCode(StatusCodes.Status503ServiceUnavailable);
        }
    }

    [Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
    [HttpPut("edit")]
    public async Task<IActionResult> EditWorkOrderAsync(WorkOrderFormDto dto)
    {
        try
        {
            await _repo.EditAsync(dto);

            return Ok(SuccessfulCrudFeedBack.WORKORDER_EDIT);
        }
        catch (NullReferenceException exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return BadRequest(exc.Message);
        }
        catch (ArgumentNullException exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return BadRequest(exc.Message);
        }
        catch (DuplicateElementException exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return BadRequest(exc.Message);
        }
        catch (DbUpdateException exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return StatusCode(StatusCodes.Status500InternalServerError);
        }
        catch (Exception exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return StatusCode(StatusCodes.Status503ServiceUnavailable);
        }
    }

    [Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
    [HttpDelete("delete/{workOrderId}")]
    public async Task<IActionResult> DeleteWorkOrderAsync(string workOrderId)
    {
        try
        {
            await _repo.DeleteAsync(workOrderId);

            return Ok(SuccessfulCrudFeedBack.WORKORDER_DELETE);
        }
        catch (InvalidOperationException exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return NotFound(exc.Message);
        }
        catch (DbUpdateException exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return StatusCode(StatusCodes.Status500InternalServerError);
        }
        catch (Exception exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return NotFound(exc.Message);
        }
    }

    [HttpGet("search/{workOrder}")]
    public async Task<List<WorkOrderSelectDto>> SearchWorkOrderAsync(string workOrderName)
    {
        try
        {
            return await _repo.SearchWorkOrderAsync(workOrderName);
        }
        catch (Exception exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return new();
        }
    }

    [Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
    [HttpGet("get/for/edit/{workOrderId}")]
    public async Task<WorkOrderFormDto> GetWorkOrderForEditByIdAsync(string workOrderId)
    {
        try
        {
            return await _repo.GetForEditByIdAsync(workOrderId);
        }
        catch (Exception exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return new();
        }
    }

    [HttpGet("get/for/view/{workOrderId}")]
    public async Task<WorkOrderViewDto> GetWorkOrderForViewByIdAsync(string workOrderId)
    {
        try
        {
            return await _repo.GetForViewByIdAsync(workOrderId);
        }
        catch (Exception exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return new();
        }
    }

    [Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
    [HttpGet("get/for/delete/{workOrderId}")]
    public async Task<WorkOrderDeleteDto> GetWorkOrderForDeleteByIdAsync(string workOrderId)
    {
        try
        {
            return await _repo.GetForDeleteByIdAsync(workOrderId);
        }
        catch (Exception exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return new();
        }
    }

    [HttpGet("get/size")]
    public async Task<int> GetSizeAsync()
    {
        try
        {
            return await _repo.GetWorkOrdersSizeAsync();
        }
        catch (Exception exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return default;
        }
    }

    [HttpGet("get/paginated/{limit}/{offset}")]
    public async Task<List<WorkOrderViewDto>> GetPaginatedWorkOrdersAsync(int limit = 0, int offset = 5)
    {
        try
        {
            return await _repo.GetPaginatedWorkOrdersAsync(limit, offset);
        }
        catch (Exception exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return new();
        }
    }
}