namespace PlannerCRM.Client.Pages.OperationManager.Home;

[Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
public partial class OperationManager : ComponentBase
{
    [Inject] public OperationManagerCrudService OperationManagerService { get; set; }

    private List<WorkOrderViewDto> _workOrders;
    private List<WorkOrderViewDto> _filteredList;
    private List<ClientViewDto> _clients;

    private Dictionary<string, Action> _filters;
    private bool _isCreateWorkOrderClicked;

    private bool _isCreateClientClicked;
    private bool _isDeleteClientClicked;

    private bool _isCreateActivityClicked;
    private bool _isShowClientsClicked;

    private int _collectionSize;

    protected override void OnInitialized()
    {
        _workOrders = new();
        _filteredList = new();
        _clients = new();
        _filters = new() {
            { "Tutte", GetAll },
            { "Attive", GetActive },
            { "Archiviate", GetArchived },
            { "Completate", GetCompleted },
            { "Eliminate", GetDeleted },
        };
    }

    protected override async Task OnInitializedAsync()
    {
        _collectionSize = await OperationManagerService.GetWorkOrdersCollectionSizeAsync();
        _workOrders = await OperationManagerService.GetPaginatedWorkOrdersAsync();
        _filteredList = new(_workOrders);
    }

    private void HandleSearchedElements(string query)
    {
        if (string.IsNullOrEmpty(query))
        {
            _filteredList = new(_workOrders);
        }

        _filteredList = _workOrders
            .Where(wo =>
                {
                    return
                        wo.Name
                            .Contains(query, StringComparison.OrdinalIgnoreCase) ||
                        wo.ClientName
                            .Contains(query, StringComparison.OrdinalIgnoreCase);
                })
            .ToList();

        StateHasChanged();
    }

    private void GetAll()
    {
        _filteredList = new(_workOrders);

        StateHasChanged();
    }

    private void GetArchived()
    {
        _filteredList = _workOrders
            .Where(wo => wo.IsArchived)
            .ToList();

        StateHasChanged();
    }

    private void GetActive()
    {
        _filteredList = _workOrders
            .Where(wo => !wo.IsDeleted && !wo.IsCompleted && !wo.IsArchived)
            .ToList();

        StateHasChanged();
    }

    private void GetCompleted()
    {
        _filteredList = _workOrders
            .Where(wo => wo.IsCompleted)
            .ToList();

        StateHasChanged();
    }

    private void GetDeleted()
    {
        _filteredList = _workOrders
            .Where(wo => wo.IsDeleted)
            .ToList();

        StateHasChanged();
    }

    public async Task HandlePaginate(int limit, int offset)
    {
        _workOrders = await OperationManagerService.GetPaginatedWorkOrdersAsync(limit, offset);

        _filteredList = new(_workOrders);

        StateHasChanged();
    }

    public async Task OnClickShowClients()
    {
        _isShowClientsClicked = !_isShowClientsClicked;
        _clients = await OperationManagerService.GetClientsPaginatedAsync();
    }

    private void OnClickAddWorkOrder() =>
       _isCreateWorkOrderClicked = !_isCreateWorkOrderClicked;

    private void OnClickAddActivity() =>
       _isCreateActivityClicked = !_isCreateActivityClicked;

    private void OnClickAddClient() =>
       _isCreateClientClicked = !_isCreateClientClicked;

    public void OnClickDeleteClient(string id)
    {
        _isDeleteClientClicked = !_isDeleteClientClicked;
    }
}