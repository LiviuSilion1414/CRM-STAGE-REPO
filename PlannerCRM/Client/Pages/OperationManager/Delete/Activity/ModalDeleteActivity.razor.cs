using PlannerCRM.Client.Utilities.Navigation;

namespace PlannerCRM.Client.Pages.OperationManager.Delete.Activity;

[Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
public partial class ModalDeleteActivity : ComponentBase
{
    [Parameter] public string Title { get; set; }
    [Parameter] public string WorkOrderId { get; set; }
    [Parameter] public string ActivityId { get; set; }

    [Inject] public OperationManagerCrudService OperationManagerService { get; set; }
    [Inject] public NavigationManager NavManager { get; set; }
    [Inject] public NavigationLockService NavigationUtil { get; set; }

    private WorkOrderViewDto _currentWorkOrder;
    private ActivityDeleteDto _currentActivity;

    private bool _isCancelClicked;
    public string _message;

    private string _currentPage;
    private bool _isError;

    protected override async Task OnInitializedAsync()
    {
        _currentWorkOrder = await OperationManagerService.GetWorkOrderForViewByIdAsync(WorkOrderId);
        _currentActivity = await OperationManagerService.GetActivityForDeleteByIdAsync(ActivityId);
    }

    protected override void OnInitialized()
    {
        _currentPage = _currentPage = NavigationUtil.GetCurrentPage();
        _currentWorkOrder = new();
        _currentActivity = new()
        {
            Employees = new()
        };
    }

    public void OnClickModalCancel()
    {
        _isCancelClicked = !_isCancelClicked;
        NavManager.NavigateTo(_currentPage);
    }

    private void OnClickHideBanner(bool hidden) => _isError = hidden;

    public async Task OnClickModalConfirm()
    {
        try
        {
            var responseDelete = await OperationManagerService.DeleteActivityAsync(ActivityId);

            if (!responseDelete.IsSuccessStatusCode)
            {
                _message = await responseDelete.Content.ReadAsstring Async();
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