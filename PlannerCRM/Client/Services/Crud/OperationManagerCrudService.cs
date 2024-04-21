namespace PlannerCRM.Client.Services.Crud;

[Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
public class OperationManagerCrudService
{
    private readonly HttpClient _http;
    private readonly ILogger<OperationManagerCrudService> _logger;

    public OperationManagerCrudService(
        HttpClient http,
        Logger<OperationManagerCrudService> logger)
    {
        _http = http;
        _logger = logger;
    }


    public async Task<HttpResponseMessage> AddClientAsync(ClientFormDto dto)
    {
        try
        {
            return await _http.PostAsJsonAsync("api/client/add", dto);
        }
        catch (Exception exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return new() { ReasonPhrase = exc.StackTrace };
        }
    }

    public async Task<HttpResponseMessage> EditClientAsync(ClientFormDto dto)
    {
        try
        {
            return await _http.PutAsJsonAsync("api/client/edit", dto);
        }
        catch (Exception exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return new() { ReasonPhrase = exc.StackTrace };
        }
    }

    public async Task<HttpResponseMessage> DeleteClientAsync(string clientId)
    {
        try
        {
            return await _http.DeleteAsync($"api/client/delete/{clientId}");
        }
        catch (Exception exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return new() { ReasonPhrase = exc.StackTrace };
        }
    }

    public async Task<ClientFormDto> GetClientForEditByIdAsync(string clientId)
    {
        try
        {
            return await _http
                .GetFromJsonAsync<ClientFormDto>($"api/client/get/for/edit/{clientId}");
        }
        catch (Exception exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return new();
        }
    }

    public async Task<ClientDeleteDto> GetClientForDeleteByIdAsync(string clientId)
    {
        try
        {
            return await _http
                .GetFromJsonAsync<ClientDeleteDto>($"api/client/get/for/delete/{clientId}");
        }
        catch (Exception exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return new();
        }
    }

    public async Task<List<ClientViewDto>> GetClientsPaginatedAsync(int limit = 0, int offset = 5)
    {
        try
        {
            return await _http
                .GetFromJsonAsync<List<ClientViewDto>>($"api/client/get/paginated/{limit}/{offset}");
        }
        catch (Exception exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return new();
        }
    }

    public async Task<int> GetWorkOrdersCollectionSizeAsync()
    {
        try
        {
            return await _http
                .GetFromJsonAsync<int>("api/workorder/get/size/");
        }
        catch (Exception exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return new();
        }
    }

    public async Task<List<WorkOrderViewDto>> GetPaginatedWorkOrdersAsync(int limit = 0, int offset = 5)
    {
        try
        {
            return await _http
                .GetFromJsonAsync<List<WorkOrderViewDto>>($"api/workorder/get/paginated/{limit}/{offset}");
        }
        catch (Exception exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return new();
        }
    }

    public async Task<List<WorkOrderSelectDto>> SearchWorkOrderAsync(string workOrder)
    {
        try
        {
            return await _http
                .GetFromJsonAsync<List<WorkOrderSelectDto>>($"api/workorder/search/{workOrder}");
        }
        catch (Exception exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return new();
        }
    }

    public async Task<List<EmployeeSelectDto>> SearchEmployeeAsync(string employee)
    {
        try
        {
            return await _http
                .GetFromJsonAsync<List<EmployeeSelectDto>>($"api/employee/search/{employee}");
        }
        catch (Exception exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return new();
        }
    }

    public async Task<HttpResponseMessage> AddWorkOrderAsync(WorkOrderFormDto dto)
    {
        try
        {
            return await _http
                .PostAsJsonAsync("api/workorder/add", dto);
        }
        catch (Exception exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return new() { ReasonPhrase = exc.StackTrace };
        }
    }

    public async Task<HttpResponseMessage> EditWorkOrderAsync(WorkOrderFormDto dto)
    {
        try
        {
            return await _http
                .PutAsJsonAsync("api/workorder/edit", dto);
        }
        catch (Exception exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return new() { ReasonPhrase = exc.StackTrace };
        }
    }

    public async Task<HttpResponseMessage> AddActivityAsync(ActivityFormDto dto)
    {
        try
        {
            return await _http
                .PostAsJsonAsync("api/activity/add", dto);
        }
        catch (Exception exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return new() { ReasonPhrase = exc.StackTrace };
        }
    }

    public async Task<HttpResponseMessage> EditActivityAsync(ActivityFormDto dto)
    {
        try
        {
            return await _http
                .PutAsJsonAsync("api/activity/edit", dto);
        }
        catch (Exception exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return new() { ReasonPhrase = exc.StackTrace };
        }
    }

    public async Task<HttpResponseMessage> DeleteActivityAsync(string activityId)
    {
        try
        {
            return await _http
                .DeleteAsync($"api/activity/delete/{activityId}");
        }
        catch (Exception exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return new() { ReasonPhrase = exc.StackTrace };
        }
    }

    public async Task<HttpResponseMessage> DeleteWorkOrderAsync(string workOrderId)
    {
        try
        {
            return await _http
                .DeleteAsync($"api/workorder/delete/{workOrderId}");
        }
        catch (Exception exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return new() { ReasonPhrase = exc.StackTrace };
        }
    }

    public async Task<ActivityFormDto> GetActivityForEditByIdAsync(string activityId)
    {
        try
        {
            return await _http
                .GetFromJsonAsync<ActivityFormDto>($"api/activity/get/for/edit/{activityId}");
        }
        catch (Exception exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return new();
        }
    }

    public async Task<ActivityDeleteDto> GetActivityForDeleteByIdAsync(string activityId)
    {
        try
        {
            return await _http
                .GetFromJsonAsync<ActivityDeleteDto>($"api/activity/get/for/delete/{activityId}");
        }
        catch (Exception exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return new();
        }
    }

    public async Task<WorkOrderViewDto> GetWorkOrderForViewByIdAsync(string workOrderId)
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

    public async Task<WorkOrderFormDto> GetWorkOrderForEditByIdAsync(string workOrderId)
    {
        try
        {
            var response = await _http.GetAsync($"api/workorder/get/for/edit/{workOrderId}");
            var jsonObject = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<WorkOrderFormDto>(jsonObject);
        }
        catch (Exception exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return new();
        }
    }

    public async Task<WorkOrderDeleteDto> GetWorkOrderForDeleteByIdAsync(string workOrderId)
    {
        try
        {
            return await _http
                .GetFromJsonAsync<WorkOrderDeleteDto>($"api/workorder/get/for/delete/{workOrderId}");
        }
        catch (Exception exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return new();
        }
    }

    public async Task<List<ActivityViewDto>> GetActivityByWorkOrderAsync(string workOrderId)
    {
        try
        {
            return await _http
                .GetFromJsonAsync<List<ActivityViewDto>>($"api/activity/get/activity/by/workorder/{workOrderId}");
        }
        catch (Exception exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return new();
        }
    }

    public async Task<int> GetClientsCollectionSizeAsync()
    {
        try
        {
            return await _http
                .GetFromJsonAsync<int>("api/client/get/size/");
        }
        catch (Exception exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return new();
        }
    }

    public async Task<List<ClientViewDto>> SearchClientAsync(string clientName)
    {
        try
        {
            return await _http
                .GetFromJsonAsync<List<ClientViewDto>>($"api/client/search/{clientName}");
        }
        catch (Exception exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return new();
        }
    }

    public async Task<List<ClientViewDto>> SearchClientByIdAsync(string clientId)
    {
        try
        {
            return await _http
                .GetFromJsonAsync<List<ClientViewDto>>($"api/client/search/by/id/{clientId}");
        }
        catch (Exception exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return new();
        }
    }
}