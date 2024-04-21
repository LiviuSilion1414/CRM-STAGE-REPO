using PlannerCRM.Client.Utilities.Navigation;

namespace PlannerCRM.Client.Pages.AccountManager.Delete;

[Authorize(Roles = nameof(Roles.ACCOUNT_MANAGER))]
public partial class ModalDeleteUser : ComponentBase
{
    [Parameter] public string Id { get; set; }
    [Parameter] public string Title { get; set; }
    [Parameter] public string ConfirmationMessage { get; set; }

    [Inject] public NavigationManager NavManager { get; set; }
    [Inject] public NavigationLockService NavigationUtil { get; set; }
    [Inject] public AccountManagerCrudService AccountManagerService { get; set; }

    private List<WorkOrderViewDto> _workOrders;
    private EmployeeDeleteDto _model;
    private string _currentPage;
    private bool _isCancelClicked;

    private string _message;
    private bool _isError;

    protected override async Task OnInitializedAsync()
    {
        _model = await AccountManagerService.GetEmployeeForDeleteByIdAsync(Id);

        foreach (var ac in _model.EmployeeActivities)
        {
            var workOrder = await AccountManagerService.GetWorkOrderForViewByIdAsync(ac.Activity.WorkOrderId);
            if (!_workOrders.Any(w => w.Id == workOrder.Id))
            {
                _workOrders.Add(workOrder);
            }
        }
    }

    protected override void OnInitialized()
    {
        _isCancelClicked = false;
        _model = new() { EmployeeActivities = new() };
        _workOrders = new();
        _currentPage = NavigationUtil.GetCurrentPage();
    }

    private void OnClickModalCancel()
    {
        _isCancelClicked = !_isCancelClicked;
        NavManager.NavigateTo(_currentPage);
    }

    private void OnClickHideBanner(bool hidden) => _isError = hidden;

    private async Task OnClickDeleteAsync()
    {
        var responseEmployee = await AccountManagerService.DeleteEmployeeAsync(Id);

        if (!responseEmployee.IsSuccessStatusCode)
        {
            _message = await responseEmployee.Content.ReadAsStringAsync();
            _isError = true;
        }
        else
        {
            _isCancelClicked = !_isCancelClicked;
            NavManager.NavigateTo(_currentPage, true);
        }
    }

    private async Task OnClickArchiveAsync()
    {
        var responseEmployee = await AccountManagerService.ArchiveEmployeeAsync(Id);

        if (!responseEmployee.IsSuccessStatusCode)
        {
            _message = await responseEmployee.Content.ReadAsStringAsync();
            _isError = true;
        }

        _isCancelClicked = !_isCancelClicked;
        NavManager.NavigateTo(_currentPage, true);
    }
}
