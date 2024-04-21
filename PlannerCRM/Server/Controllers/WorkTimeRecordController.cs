namespace PlannerCRM.Server.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class WorkTimeRecordController : ControllerBase
{
    private readonly WorkTimeRecordRepository _repo;
    private readonly ILogger<WorkTimeRecordRepository> _logger;

    public WorkTimeRecordController(
        WorkTimeRecordRepository repo,
        Logger<WorkTimeRecordRepository> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddWorkTimeRecordAsync(WorkTimeRecordFormDto dto)
    {
        try
        {
            await _repo.AddAsync(dto);

            return Ok(SuccessfulCrudFeedBack.WORKTIMERECORD_ADD);
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
        catch (DuplicateElementException exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return BadRequest(exc.Message);
        }
        catch (DbUpdateException exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return BadRequest(exc.Message);
        }
        catch (Exception exc)
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable, exc.Message);
        }
    }

    [Authorize(Roles = nameof(Roles.SENIOR_DEVELOPER))]
    [Authorize(Roles = nameof(Roles.JUNIOR_DEVELOPER))]
    [HttpPut("edit")]
    public async Task<IActionResult> EditWorkTimeRecordAsync(WorkTimeRecordFormDto dto)
    {
        try
        {
            await _repo.EditAsync(dto);

            return Ok(SuccessfulCrudFeedBack.WORKTIMERECORD_EDIT);
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
        catch (KeyNotFoundException exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return NotFound(exc.Message);
        }
        catch (DuplicateElementException exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return BadRequest(exc.Message);
        }
        catch (DbUpdateException exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return BadRequest(exc.Message);
        }
        catch (Exception exc)
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable, exc.Message);
        }
    }

    [HttpGet("get/{workOrderId}/{activityId}/{employeeId}")]
    public async Task<WorkTimeRecordViewDto> GetWorkTimeRecordAsync(string workOrderId, string activityId, string employeeId)
    {
        try
        {
            return await _repo.GetAsync(workOrderId, activityId, employeeId);
        }
        catch (KeyNotFoundException exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return new();
        }
    }

    [HttpGet("get/paginated/{limit}/{offset}")]
    public async Task<List<WorkTimeRecordViewDto>> GetPaginatedWorkTimeRecordsAsync(int limit, int offset)
    {
        try
        {
            return await _repo.GetPaginatedWorkTimeRecordsAsync(limit, offset);
        }
        catch (Exception exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return new();
        }
    }

    [HttpGet("get/by/employee/{employeeId}")]
    public async Task<WorkTimeRecordViewDto> GetAllWorkTimeRecordsByEmployeeIdAsync(string employeeId)
    {
        try
        {
            return await _repo.GetAllWorkTimeRecordsByEmployeeIdAsync(employeeId);
        }
        catch (Exception exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return new();
        }
    }
}
