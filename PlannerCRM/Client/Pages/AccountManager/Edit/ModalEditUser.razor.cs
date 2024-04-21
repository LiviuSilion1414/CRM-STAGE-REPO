using PlannerCRM.Client.Utilities.Navigation;

namespace PlannerCRM.Client.Pages.AccountManager.Edit;

[Authorize(Roles = nameof(Roles.ACCOUNT_MANAGER))]
public partial class ModalEditUser : ComponentBase
{
    [Parameter] public string Title { get; set; }
    [Parameter] public string Id { get; set; }

    [Inject] public AccountManagerCrudService AccountManagerService { get; set; }
    [Inject] public NavigationManager NavManager { get; set; }
    [Inject] public NavigationLockService NavigationUtil { get; set; }

    private EmployeeFormDto _model;
    private string _currentPage;

    protected override async Task OnInitializedAsync() =>
        _model = await AccountManagerService.GetEmployeeForEditByIdAsync(Id);

    protected override void OnInitialized()
    {
        _model = new();
        _currentPage = NavigationUtil.GetCurrentPage();
    }

    private async Task OnClickModalConfirm(EmployeeFormDto returnedModel)
    {
        await AccountManagerService.UpdateEmployeeAsync(returnedModel);

        NavManager.NavigateTo(_currentPage, true);
    }
}