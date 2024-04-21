using PlannerCRM.Client.Utilities.Navigation;

namespace PlannerCRM.Client.Pages.AccountManager.Details;

[Authorize(Roles = nameof(Roles.ACCOUNT_MANAGER))]
public partial class ModalShowUserDetails : ComponentBase
{
    [Parameter] public string Id { get; set; }
    [Parameter] public string Title { get; set; }

    [Inject] public NavigationManager NavManager { get; set; }
    [Inject] public AccountManagerCrudService AccountManagerService { get; set; }
    [Inject] public NavigationLockService NavigationUtil { get; set; }

    private readonly bool _isDisabled = true;

    private EmployeeViewDto _model;
    private bool _isCancelClicked;

    private string _currentPage;
    private string _input;

    protected override async Task OnInitializedAsync() =>
        _model = await AccountManagerService.GetEmployeeForViewByIdAsync(Id);

    protected override void OnInitialized()
    {
        _model = new()
        {
            ProfilePicture = new()
            {
                EmployeeInfo = new()
            }
        };
        _currentPage = NavigationUtil.GetCurrentPage();
    }

    private void OnClickModalCancel()
    {
        _isCancelClicked = !_isCancelClicked;
        NavManager.NavigateTo(_currentPage);
    }

    private void SwitchPassword(string type) => _input = type;
}