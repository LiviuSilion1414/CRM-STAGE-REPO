namespace PlannerCRM.Client.Pages.Modals.Form.List;

[Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
public partial class ClientsList : ComponentBase
{
    [Parameter] public string Title { get; set; }

    [Inject] public OperationManagerCrudService OperationManagerService { get; set; }

    private List<ClientViewDto> _clients;
    private List<WorkOrderViewDto> _workOrders;

    private bool _isCreateClientClicked;
    private bool _isEditClientClicked;
    private bool _isDeleteClientClicked;

    private bool _isCancelClicked;

    private string _clientId;
    private int _collectionSize;

    protected override async Task OnInitializedAsync()
    {
        _collectionSize = await OperationManagerService.GetClientsCollectionSizeAsync();
        _clients = await OperationManagerService.GetClientsPaginatedAsync();
    }

    protected override void OnInitialized()
    {
        _clients = new();
        _workOrders = new();
    }

    public async Task HandlePaginate(int limit, int offset) =>
        _clients = await OperationManagerService.GetClientsPaginatedAsync(limit, offset);

    private void OnClickModalCancel() =>
         _isCancelClicked = !_isCancelClicked;

    private void OnClickAddClient() =>
       _isCreateClientClicked = !_isCreateClientClicked;

    private void OnClickEditClient(string id)
    {
        _isEditClientClicked = !_isEditClientClicked;
        _clientId = id;
    }

    public void OnClickDeleteClient(string id)
    {
        _isDeleteClientClicked = !_isDeleteClientClicked;
        _clientId = id;
    }
}