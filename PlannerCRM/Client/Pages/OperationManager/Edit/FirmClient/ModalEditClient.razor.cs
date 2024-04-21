using PlannerCRM.Client.Utilities.Navigation;

namespace PlannerCRM.Client.Pages.OperationManager.Edit.FirmClient;

[Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
public partial class ModalEditClient : ComponentBase
{
    [Parameter] public string Title { get; set; }
    [Parameter] public string Id { get; set; }

    [Inject] public OperationManagerCrudService OperationManagerService { get; set; }
    [Inject] public NavigationLockService NavigationUtil { get; set; }
    [Inject] public NavigationManager NavManager { get; set; }

    private ClientFormDto _model;
    private string _currentPage;

    protected override async Task OnInitializedAsync()
        => _model = await OperationManagerService.GetClientForEditByIdAsync(Id);

    protected override void OnInitialized()
    {
        _model = new();
        _currentPage = NavigationUtil.GetCurrentPage();
    }

    public async Task OnClickModalConfirm(ClientFormDto returnedModel)
    {
        await OperationManagerService.EditClientAsync(returnedModel);

        NavManager.NavigateTo(_currentPage, true);
    }
}