using Microsoft.JSInterop;

namespace PlannerCRM.Client.Components.Invoices;

[Authorize(Roles = nameof(Roles.PROJECT_MANAGER))]
public partial class InvoiceViewer : ComponentBase
{
    [Parameter] public string WorkOrderId { get; set; }
    [Parameter] public string Title { get; set; }

    [Inject] public IJSRuntime JSRuntime { get; set; }
    [Inject] public ProjectManagerService ProjectManagerService { get; set; }

    private WorkOrderCostDto _invoice;
    private ClientViewDto _client;

    protected override async Task OnInitializedAsync()
    {
        _invoice = await ProjectManagerService.GetInvoiceAsync(WorkOrderId);
        _client = await ProjectManagerService.GetClientForViewByIdAsync(_invoice.ClientId);
    }

    protected override void OnInitialized()
    {
        _client = new();
        _invoice = new()
        {
            Activities = new(),
            Employees = new(),
            MonthlyActivityCosts = new()
        };
    }
}