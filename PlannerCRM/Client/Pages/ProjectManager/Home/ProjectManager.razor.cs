using PlannerCRM.Client.Utilities.Navigation;

namespace PlannerCRM.Client.Pages.ProjectManager.Home;

[Authorize(Roles = nameof(Roles.PROJECT_MANAGER))]
public partial class ProjectManager : ComponentBase
{
    [Inject] public ProjectManagerService ProjectManagerService { get; set; }
    [Inject] public NavigationLockService NavigationUtil { get; set; }
    [Inject] public NavigationManager NavManager { get; set; }
    [Inject] public ILogger<ProjectManager> Logger { get; set; }

    private List<WorkOrderViewDto> _workOrders;
    private List<WorkOrderViewDto> _filteredList;

    private Dictionary<string, Action> _filters => new() {
        { "Tutti", OnClickAll },
        { "Creati", OnClickSortCreated },
        { "Inesistenti", OnClickSortNotCreated }
    };

    private int _collectionSize;

    protected override void OnInitialized()
    {
        _filteredList = new();
        _workOrders = new();
    }

    protected override async Task OnInitializedAsync()
        => await FetchDataAsync();

    private async Task FetchDataAsync(int limit = 0, int offset = 5)
    {
        _collectionSize = await ProjectManagerService.GetWorkOrderCostsCollectionSizeAsync();
        _workOrders = await ProjectManagerService.GetWorkOrdersCostsPaginatedAsync(limit, offset);

        _filteredList = new(_workOrders);
    }

    private void OnClickAll()
    {
        _filteredList = new(_workOrders);

        StateHasChanged();
    }

    private void HandleSearchedElements(string query)
    {
        _filteredList = _workOrders
            .Where(wo => wo.Name.Contains(query, string Comparison.OrdinalIgnoreCase))
            .ToList();

        StateHasChanged();
    }

    private void OnClickSortCreated()
    {
        try
        {
            _filteredList = _workOrders
                .Where(wo => wo.IsInvoiceCreated)
                .ToList();

            StateHasChanged();

        }
        catch (Exception exc)
        {
            Logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);
        }
    }

    private void OnClickSortNotCreated()
    {
        try
        {
            _filteredList = _workOrders
                .Where(wo => !wo.IsInvoiceCreated)
                .ToList();

            StateHasChanged();
        }
        catch (Exception exc)
        {
            Logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);
        }
    }

    private async Task HandlePaginate(int limit, int offset) =>
       await FetchDataAsync(limit, offset);
}