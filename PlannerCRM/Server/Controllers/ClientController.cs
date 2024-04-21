namespace PlannerCRM.Server.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ClientController : ControllerBase
{
    private readonly ClientRepository _repo;
    private readonly ILogger<ClientRepository> _logger;

    public ClientController(
        ClientRepository repo,
        Logger<ClientRepository> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    [Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
    [HttpPost("add")]
    public async Task<IActionResult> AddClientAsync(ClientFormDto dto)
    {
        try
        {
            await _repo.AddClientAsync(dto);

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

    [Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
    [HttpPut("edit")]
    public async Task<IActionResult> EditClientAsync(ClientFormDto dto)
    {
        try
        {
            await _repo.EditClientAsync(dto);

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

    [Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
    [HttpDelete("delete/{clientId}")]
    public async Task<IActionResult> DeleteClientAsync(string clientId)
    {
        try
        {
            await _repo.DeleteClientAsync(clientId);

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

    [HttpGet("get/for/view/{clientId}")]
    public async Task<ClientViewDto> GetClientForViewByIdAsync(string clientId)
    {
        try
        {
            return await _repo.GetClientForViewByIdAsync(clientId);
        }
        catch (Exception exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return new();
        }
    }

    [Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
    [HttpGet("get/for/edit/{clientId}")]
    public async Task<ClientFormDto> GetClientForEditByIdAsync(string clientId)
    {
        try
        {
            return await _repo.GetClientForEditByIdAsync(clientId);
        }
        catch (Exception exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return new();
        }
    }

    [Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
    [HttpGet("get/for/delete/{clientId}")]
    public async Task<ClientDeleteDto> GetClientForDeleteByIdAsync(string clientId)
    {
        try
        {
            return await _repo.GetClientForDeleteByIdAsync(clientId);
        }
        catch (Exception exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return new();
        }
    }


    [HttpGet("get/paginated/{limit}/{offset}")]
    public async Task<List<ClientViewDto>> GetPaginatedClientsAsync(int limit, int offset)
    {
        try
        {
            return await _repo.GetClientsPaginatedAsync(limit, offset);
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
            return await _repo.GetCollectionSizeAsync();
        }
        catch (Exception exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return new();
        }
    }

    [HttpGet("search/{clientName}/")]
    public async Task<List<ClientViewDto>> SearchClientByNameAsync(string clientName)
    {
        try
        {
            return await _repo.SearchClientAsync(clientName);
        }
        catch (Exception exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return new();
        }
    }

    [HttpGet("search/by/id/{clientId}")]
    public async Task<List<ClientViewDto>> SearchClientByIdAsync(string clientId)
    {
        try
        {
            return await _repo.SearchClientAsync(clientId);
        }
        catch (Exception exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return new();
        }
    }
}