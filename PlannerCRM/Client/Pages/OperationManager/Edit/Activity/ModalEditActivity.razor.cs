using PlannerCRM.Client.Utilities.Navigation;

namespace PlannerCRM.Client.Pages.OperationManager.Edit.Activity;

[Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
public partial class ModalEditActivity : ComponentBase
{
    [Parameter] public string Title { get; set; }
    [Parameter] public string WorkOrderId { get; set; }
    [Parameter] public string ActivityId { get; set; }

    [Inject] public OperationManagerCrudService OperationManagerService { get; set; }
    [Inject] public NavigationManager NavManager { get; set; }
    [Inject] public NavigationLockService NavigationUtil { get; set; }

    private ActivityFormDto _model;
    private WorkOrderViewDto _currentWorkOrder;
    private string _currentPage;

    protected override async Task OnInitializedAsync()
    {
        _model = await OperationManagerService.GetActivityForEditByIdAsync(ActivityId);
        _currentWorkOrder = await OperationManagerService.GetWorkOrderForViewByIdAsync(WorkOrderId);
        _model.SelectedWorkOrder = _currentWorkOrder.Name;
        _model.SelectedEmployee = string.Empty;
    }

    protected override void OnInitialized()
    {
        _currentWorkOrder = new();
        _model = new()
        {
            ViewEmployeeActivity = new(),
            EmployeeActivity = new(),
            DeleteEmployeeActivity = new()
        };

        _currentPage = NavigationUtil.GetCurrentPage();
    }

    private async Task OnClickModalConfirm(ActivityFormDto returnedModel)
    {
        returnedModel.SelectedEmployee = string.Empty;

        await OperationManagerService.EditActivityAsync(returnedModel);

        NavManager.NavigateTo(_currentPage, true);
    }
}