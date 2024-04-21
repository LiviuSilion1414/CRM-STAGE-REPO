using PlannerCRM.Client.Utilities.Navigation;

namespace PlannerCRM.Client.Pages.AccountManager.Add;

[Authorize(Roles = nameof(Roles.ACCOUNT_MANAGER))]
public partial class ModalAddUser : ComponentBase
{
    [Parameter] public string Title { get; set; }

    [Inject] public AccountManagerCrudService AccountManagerService { get; set; }
    [Inject] public NavigationLockService NavigationUtil { get; set; }
    [Inject] public NavigationManager NavManager { get; set; }

    private EmployeeFormDto _model;
    private string _currentPage;

    protected override void OnInitialized()
    {
        _model = new();
        _currentPage = NavigationUtil.GetCurrentPage();
    }

    private async Task OnClickAddAsync(EmployeeFormDto returnedModel)
    {
        await AccountManagerService.AddEmployeeAsync(returnedModel);

        NavManager.NavigateTo(_currentPage, true);
    }
}