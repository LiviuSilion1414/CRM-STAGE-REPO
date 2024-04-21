using PlannerCRM.Client.Utilities.Navigation;

namespace PlannerCRM.Client.Pages.ProjectManager.CostPreview;

[Authorize(Roles = nameof(Roles.PROJECT_MANAGER))]
public partial class ModalWorkOrderCostPreview : ComponentBase
{
    [Parameter] public string WorkOrderId { get; set; }
    [Parameter] public string Title { get; set; }

    [Inject] public ProjectManagerService ProjectManagerService { get; set; }
    [Inject] public NavigationLockService NavigationUtil { get; set; }
    [Inject] public NavigationManager NavManager { get; set; }

    private WorkOrderCostDto _invoice;
    private ClientViewDto _client;
    private bool _isCancelClicked = false;
    private bool _isInvoiceClicked = false;
    private string _currentPage;

    protected override async Task OnInitializedAsync()
    {
        _invoice = await ProjectManagerService.GetInvoiceAsync(WorkOrderId);
        _client = await ProjectManagerService.GetClientForViewByIdAsync(_invoice.ClientId);
    }

    protected override void OnInitialized()
    {
        _currentPage = NavigationUtil.GetCurrentPage();
        _client = new();
        _invoice = new()
        {
            Activities = new(),
            Employees = new(),
            MonthlyActivityCosts = new()
        };
    }

    private void OnClickModalCancel()
    {
        _isCancelClicked = !_isCancelClicked;
        NavManager.NavigateTo(_currentPage);
    }

    private void OnClickIssueInvoice() =>
        _isInvoiceClicked = !_isInvoiceClicked;
}