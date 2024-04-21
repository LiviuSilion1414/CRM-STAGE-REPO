namespace PlannerCRM.Server.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class EmployeeController : ControllerBase
{
    private readonly EmployeeRepository _repo;
    private readonly ILogger<EmployeeRepository> _logger;
    private readonly UserManager<IdentityUser> _userManager;

    public EmployeeController(
        EmployeeRepository repo,
        Logger<EmployeeRepository> logger,
        UserManager<IdentityUser> userManager)
    {
        _repo = repo;
        _logger = logger;
        _userManager = userManager;
    }

    [Authorize(Roles = nameof(Roles.ACCOUNT_MANAGER))]
    [HttpPost("add")]
    public async Task<IActionResult> AddEmployeeAsync(EmployeeFormDto dto)
    {
        try
        {
            await _repo.AddAsync(dto);

            return Ok(SuccessfulCrudFeedBack.USER_ADD);
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

    [Authorize]
    [HttpPut("edit")]
    public async Task<IActionResult> EditEmployeeAsync(EmployeeFormDto dto)
    {
        try
        {
            await _repo.EditAsync(dto);

            return Ok(SuccessfulCrudFeedBack.USER_EDIT);
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

            return StatusCode(StatusCodes.Status503ServiceUnavailable);
        }
    }

    [Authorize(Roles = nameof(Roles.ACCOUNT_MANAGER))]
    [HttpDelete("delete/{employeeId}")]
    public async Task<IActionResult> DeleteEmployeeAsync(string employeeId)
    {
        try
        {
            await _repo.DeleteAsync(employeeId);

            return Ok(SuccessfulCrudFeedBack.USER_DELETE);
        }
        catch (InvalidOperationException exc)
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
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return StatusCode(StatusCodes.Status503ServiceUnavailable);
        }
    }

    [Authorize(Roles = nameof(Roles.ACCOUNT_MANAGER))]
    [HttpGet("archive/{employeeId}")]
    public async Task<IActionResult> ArchiveEmployeeAsync(string employeeId)
    {
        try
        {
            await _repo.ArchiveAsync(employeeId);

            return Ok(SuccessfulCrudFeedBack.USER_ARCHIVE);
        }
        catch (InvalidOperationException exc)
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
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return StatusCode(StatusCodes.Status503ServiceUnavailable);
        }
    }

    [Authorize(Roles = nameof(Roles.ACCOUNT_MANAGER))]
    [HttpGet("restore/{employeeId}")]
    public async Task<IActionResult> RestoreEmployeeAsync(string employeeId)
    {
        try
        {
            await _repo.RestoreAsync(employeeId);

            return Ok(SuccessfulCrudFeedBack.USER_RESTORE);
        }
        catch (InvalidOperationException exc)
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
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return StatusCode(StatusCodes.Status503ServiceUnavailable);
        }
    }

    [Authorize(Roles = nameof(Roles.ACCOUNT_MANAGER))]
    [HttpGet("get/for/restore/{employeeId}")]
    public async Task<EmployeeSelectDto> GetEmployeeForRestoreByIdAsync(string employeeId)
    {
        try
        {
            return await _repo.GetForRestoreAsync(employeeId);
        }
        catch (Exception exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return new();
        }
    }

    [HttpGet("get/for/view/{employeeId}")]
    public async Task<EmployeeViewDto> GetEmployeeForViewByIdAsync(string employeeId)
    {
        try
        {
            return await _repo.GetForViewByIdAsync(employeeId);
        }
        catch (Exception exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return new();
        }
    }

    [HttpGet("get/for/edit/{employeeId}")]
    public async Task<EmployeeFormDto> GetEmployeeForEditByIdAsync(string employeeId)
    {
        try
        {
            return await _repo.GetForEditByIdAsync(employeeId);
        }
        catch (Exception exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return new();
        }
    }

    [Authorize(Roles = nameof(Roles.ACCOUNT_MANAGER))]
    [HttpGet("get/for/delete/{employeeId}")]
    public async Task<EmployeeDeleteDto> GetEmployeeForDeleteByIdAsync(string employeeId)
    {
        try
        {
            return await _repo.GetForDeleteByIdAsync(employeeId);
        }
        catch (Exception exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return new();
        }
    }

    [HttpGet("search/{email}")]
    public async Task<List<EmployeeSelectDto>> SearchEmployeeAsync(string email)
    {
        try
        {
            return await _repo.SearchEmployeeAsync(email);
        }
        catch (Exception exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return new();
        }
    }

    [HttpGet("get/paginated/{limit}/{offset}")]
    public async Task<List<EmployeeViewDto>> GetPaginatedEmployeesAsync(int limit, int offset)
    {
        try
        {
            return await _repo.GetPaginatedEmployeesAsync(limit, offset);
        }
        catch (Exception exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return new();
        }
    }

    [HttpGet("get/id/{email}")]
    public async Task<CurrentEmployeeDto> GetEmployeeIdAsync(string email)
    {
        try
        {
            return await _repo.GetEmployeeIdAsync(email);
        }
        catch (Exception exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return new();
        }
    }

    [HttpGet("get/id-check/{email}")]
    public async Task<string> GetUserIdCheckAsync(string email)
    {
        try
        {
            return (await _repo.GetEmployeeIdAsync(email)).Id;
        }
        catch (Exception exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return default;
        }
    }

    [HttpGet("get/size")]
    public async Task<int> GetEmployeesSizeAsync()
    {
        try
        {
            return await _repo.GetEmployeesSizeAsync();
        }
        catch (Exception exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return default;
        }
    }
}