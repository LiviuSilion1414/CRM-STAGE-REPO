using PlannerCRM.Client.Utilities.Navigation;

namespace PlannerCRM.Client.Pages.OperationManager.Add.Activity;

[Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
public partial class ModalAddActivity : ComponentBase
{
    [Parameter] public string Title { get; set; }

    [Inject] public OperationManagerCrudService OperationManagerService { get; set; }
    [Inject] public NavigationLockService NavigationUtil { get; set; }
    [Inject] public NavigationManager NavManager { get; set; }

    private ActivityFormDto _model;
    private string _currentPage;

    protected override void OnInitialized()
    {
        _currentPage = NavigationUtil.GetCurrentPage();
        _model = new()
        {
            EmployeeActivity = new(),
            ViewEmployeeActivity = new(),
            DeleteEmployeeActivity = new()
        };
    }

    private async Task OnClickModalConfirm(ActivityFormDto returnedModel)
    {
        await OperationManagerService.AddActivityAsync(returnedModel);

        NavManager.NavigateTo(_currentPage, true);
    }
}