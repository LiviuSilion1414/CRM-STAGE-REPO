namespace PlannerCRM.Client.Services.Crud;

[Authorize(Roles = nameof(Roles.JUNIOR_DEVELOPER))]
[Authorize(Roles = nameof(Roles.SENIOR_DEVELOPER))]
public class DeveloperService
{
    private readonly HttpClient _http;
    private readonly ILogger<DeveloperService> _logger;

    public DeveloperService(
        HttpClient http,
        Logger<DeveloperService> logger)
    {
        _http = http;
        _logger = logger;
    }

    public async Task<HttpResponseMessage> AddWorkedHoursAsync(WorkTimeRecordFormDto dto)
    {
        try
        {
            return await _http
                .PostAsJsonAsync("api/worktimerecord/add", dto);
        }
        catch (Exception exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return new() { ReasonPhrase = exc.StackTrace };
        }
    }

    public async Task<ActivityViewDto> GetActivityByIdAsync(string activityId)
    {
        try
        {
            return await _http
                .GetFromJsonAsync<ActivityViewDto>($"api/activity/get/{activityId}");
        }
        catch (Exception exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return new();
        }
    }

    public async Task<WorkOrderViewDto> GetWorkOrderByIdAsync(string workOrderId)
    {
        try
        {
            return await _http
                .GetFromJsonAsync<WorkOrderViewDto>($"api/workorder/get/for/view/{workOrderId}");
        }
        catch (Exception exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return new();
        }
    }

    public async Task<List<ActivityViewDto>> GetActivitiesByEmployeeIdAsync(string employeeId, int limit = 0, int offset = 5)
    {
        try
        {
            return await _http
                .GetFromJsonAsync<List<ActivityViewDto>>($"api/activity/get/activity/by/employee/{employeeId}/{limit}/{offset}");
        }
        catch (Exception exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return new();
        }
    }

    public async Task<int> GetWorkTimesSizeByEmployeeIdAsync(string employeeId)
    {
        try
        {
            return await _http
                .GetFromJsonAsync<int>($"api/worktimerecord/get/size/by/employee/{employeeId}");
        }
        catch (Exception exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return default;
        }
    }

    public async Task<List<WorkTimeRecordViewDto>> GetAllWorkTimeRecordsByEmployeeIdAsync(string employeeId)
    {
        try
        {
            return await _http
                .GetFromJsonAsync<List<WorkTimeRecordViewDto>>($"api/worktimerecord/get/by/employee/{employeeId}");
        }
        catch (Exception exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return new();
        }
    }

    public async Task<WorkTimeRecordViewDto> GetWorkTimeRecordsAsync(string workOrderId, string activityId, string employeeId)
    {
        try
        {
            return await _http
                .GetFromJsonAsync<WorkTimeRecordViewDto>($"api/worktimerecord/get/{workOrderId}/{activityId}/{employeeId}");
        }
        catch (Exception exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return new();
        }
    }

    public async Task<int> GetCollectionSizeByEmployeeIdAsync(string employeeId)
    {
        try
        {
            return await _http
                .GetFromJsonAsync<int>($"api/activity/get/size/by/employee/id/{employeeId}");
        }
        catch (Exception exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return new();
        }
    }
}
