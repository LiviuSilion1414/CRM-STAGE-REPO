using PlannerCRM.Client.Utilities.Navigation;

namespace PlannerCRM.Client.Pages.OperationManager.Delete.WorkOrder;

[Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
public partial class ModalDeleteWorkOrder : ComponentBase
{
    [Parameter] public string Id { get; set; }
    [Parameter] public string Title { get; set; }

    [Inject] public OperationManagerCrudService OperationManagerService { get; set; }
    [Inject] public NavigationManager NavManager { get; set; }
    [Inject] public NavigationLockService NavigationUtil { get; set; }

    private WorkOrderDeleteDto _model;

    private bool _isCancelClicked;
    private string _message;

    private string _currentPage;
    private bool _isError;

    protected override async Task OnInitializedAsync() =>
        _model = await OperationManagerService.GetWorkOrderForDeleteByIdAsync(Id);

    protected override void OnInitialized()
    {
        _model = new();
        _currentPage = NavigationUtil.GetCurrentPage();
    }

    public void OnClickModalCancel()
    {
        _isCancelClicked = !_isCancelClicked;

        NavManager.NavigateTo(_currentPage);
    }

    private void OnClickHideBanner(bool hidden)
        => _isError = hidden;

    private async Task OnClickModalConfirm()
    {
        try
        {
            var responseDelete = await OperationManagerService.DeleteWorkOrderAsync(Id);

            if (!responseDelete.IsSuccessStatusCode)
            {
                _message = await responseDelete.Content.ReadAsStringAsync();
                _isError = true;
            }
            else
            {
                _isCancelClicked = !_isCancelClicked;
                NavManager.NavigateTo(_currentPage, true);
            }
        }
        catch (Exception exc)
        {
            _isError = true;
            _message = exc.Message;
        }
    }
}