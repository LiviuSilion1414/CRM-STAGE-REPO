namespace PlannerCRM.Server.Controllers;


[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ActivityController : ControllerBase
{
    private readonly ActivityRepository _repo;
    private readonly ILogger<ActivityRepository> _logger;

    public ActivityController(
        ActivityRepository repo,
        Logger<ActivityRepository> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    [Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
    [HttpPost("add")]
    public async Task<IActionResult> AddActivityAsync(ActivityFormDto dto)
    {
        try
        {
            await _repo.AddAsync(dto);

            return Ok(SuccessfulCrudFeedBack.ACTIVITY_ADD);
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
    [HttpPut("edit")]
    public async Task<IActionResult> EditActivityAsync(ActivityFormDto dto)
    {
        try
        {
            await _repo.EditAsync(dto);

            return Ok(SuccessfulCrudFeedBack.ACTIVITY_EDIT);
        }
        catch (NullReferenceException exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return NotFound(exc.Message);
        }
        catch (ArgumentNullException exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return BadRequest(exc.Message);
        }
        catch (KeyNotFoundException exc)
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

            return BadRequest(exc.Message);
        }
    }

    [Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
    [HttpDelete("delete/{activityId}")]
    public async Task<IActionResult> DeleteActivityAsync(string activityId)
    {
        try
        {
            await _repo.DeleteAsync(activityId);

            return Ok(SuccessfulCrudFeedBack.ACTIVITY_DELETE);
        }
        catch (InvalidOperationException exc)
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

    [Authorize]
    [HttpGet("get/{activityId}")]
    public async Task<ActivityViewDto> GetActivityForViewByIdAsync(string activityId)
    {
        try
        {
            return await _repo.GetForViewByIdAsync(activityId);
        }
        catch (Exception exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return new();
        }
    }

    [Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
    [HttpGet("get/for/edit/{activityId}")]
    public async Task<ActivityFormDto> GetActivityForEditByIdAsync(string activityId)
    {
        try
        {
            return await _repo.GetForEditByIdAsync(activityId);
        }
        catch (Exception exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return new();
        }
    }

    [Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
    [HttpGet("get/for/delete/{activityId}")]
    public async Task<ActivityDeleteDto> GetActivityForDelete(string activityId)
    {
        try
        {
            return await _repo.GetForDeleteByIdAsync(activityId);
        }
        catch (Exception exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return new();
        }
    }

    [Authorize]
    [HttpGet("get/activity/by/workorder/{workOrderId}")]
    public async Task<List<ActivityViewDto>> GetActivitiesPerWorkOrderAsync(string workOrderId)
    {
        try
        {
            return await _repo.GetActivitiesPerWorkOrderAsync(workOrderId);
        }
        catch (Exception exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return new();
        }
    }

    [Authorize]
    [HttpGet("get/activity/by/employee/{employeeId}/{limit}/{offset}")]
    public async Task<List<ActivityViewDto>> GetActivitiesPerEmployeeAsync(string employeeId, int limit, int offset)
    {
        try
        {
            return await _repo.GetActivityByEmployeeId(employeeId, limit, offset);
        }
        catch (Exception exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return new();
        }
    }

    [Authorize]
    [HttpGet("get/size/by/employee/id/{employeeId}")]
    public async Task<int> GetCollectionSizeByEmployeeIdAsync(string employeeId)
    {
        try
        {
            return await _repo.GetCollectionSizeByEmployeeIdAsync(employeeId);
        }
        catch (Exception exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return new();
        }
    }
}