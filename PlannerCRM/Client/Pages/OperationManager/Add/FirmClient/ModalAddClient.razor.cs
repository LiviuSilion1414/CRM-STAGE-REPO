using PlannerCRM.Client.Utilities.Navigation;

namespace PlannerCRM.Client.Pages.OperationManager.Add.FirmClient;

[Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
public partial class ModalAddClient : ComponentBase
{
    [Parameter] public string Title { get; set; }

    [Inject] public OperationManagerCrudService OperationManagerService { get; set; }
    [Inject] public NavigationLockService NavigationUtil { get; set; }
    [Inject] public NavigationManager NavManager { get; set; }

    private ClientFormDto _model;
    private string _currentPage;

    protected override void OnInitialized()
    {
        _currentPage = NavigationUtil.GetCurrentPage();
        _model = new();
    }

    private async Task OnClickModalConfirm(ClientFormDto returnedModel)
    {
        await OperationManagerService.AddClientAsync(returnedModel);

        NavManager.NavigateTo(_currentPage, true);
    }
}