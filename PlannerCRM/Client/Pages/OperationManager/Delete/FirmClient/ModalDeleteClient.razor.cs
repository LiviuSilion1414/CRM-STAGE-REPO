using PlannerCRM.Client.Utilities.Navigation;

namespace PlannerCRM.Client.Pages.OperationManager.Delete.FirmClient;

[Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
public partial class ModalDeleteClient : ComponentBase
{
    [Parameter] public string Id { get; set; }
    [Parameter] public string Title { get; set; }
    [Parameter] public string ConfirmationMessage { get; set; }

    [Inject] public NavigationManager NavManager { get; set; }
    [Inject] public NavigationLockService NavigationUtil { get; set; }
    [Inject] public OperationManagerCrudService OperationManagerService { get; set; }

    private WorkOrderViewDto _workOrder;
    private ClientDeleteDto _model;

    private string _currentPage;
    private bool _isCancelClicked;

    private string _message;
    private bool _isError;

    protected override async Task OnInitializedAsync()
    {
        _model = await OperationManagerService.GetClientForDeleteByIdAsync(Id);
        _workOrder = await OperationManagerService.GetWorkOrderForViewByIdAsync(_model.WorkOrderId);
    }

    protected override void OnInitialized()
    {
        _currentPage = NavigationUtil.GetCurrentPage();
        _workOrder = new();
        _model = new();
    }

    private void OnClickModalCancel()
    {
        _isCancelClicked = !_isCancelClicked;
        NavManager.NavigateTo(_currentPage);
    }

    private void OnClickHideBanner(bool hidden) => _isError = hidden;

    private async Task OnClickDelete()
    {
        var responseEmployee = await OperationManagerService.DeleteClientAsync(Id);

        if (!responseEmployee.IsSuccessStatusCode)
        {
            _message = await responseEmployee.Content.ReadAsStringAsync();
            _isError = true;
        }

        _isCancelClicked = !_isCancelClicked;
        NavManager.NavigateTo(_currentPage, true);
    }
}
