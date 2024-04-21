using PlannerCRM.Client.Utilities.Navigation;

namespace PlannerCRM.Client.Pages.OperationManager.Edit.WorkOrder;

[Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
public partial class ModalEditWorkOrder : ComponentBase
{
    [Parameter] public string Id { get; set; }
    [Parameter] public string Title { get; set; }

    [Inject] public OperationManagerCrudService OperationManagerService { get; set; }
    [Inject] public NavigationManager NavManager { get; set; }
    [Inject] public NavigationLockService NavigationUtil { get; set; }

    private WorkOrderFormDto _model;
    private string _currentPage;

    protected override async Task OnInitializedAsync() =>
        _model = await OperationManagerService.GetWorkOrderForEditByIdAsync(Id);

    protected override void OnInitialized()
    {
        _currentPage = NavigationUtil.GetCurrentPage();
        _model = new();
    }

    private async Task OnClickModalConfirm(WorkOrderFormDto returnedModel)
    {
        await OperationManagerService.EditWorkOrderAsync(returnedModel);

        NavManager.NavigateTo(_currentPage, true);
    }

}