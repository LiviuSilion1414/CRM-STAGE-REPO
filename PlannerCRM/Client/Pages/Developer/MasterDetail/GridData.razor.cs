namespace PlannerCRM.Client.Pages.Developer.MasterDetail;

[Authorize(Roles = nameof(Roles.SENIOR_DEVELOPER))]
[Authorize(Roles = nameof(Roles.JUNIOR_DEVELOPER))]
public partial class GridData : ComponentBase
{
    [Parameter] public string EmployeeId { get; set; }

    [Inject] public DeveloperService DeveloperService { get; set; }

    private Dictionary<string, Action> _filters;
    private Dictionary<string, Action> _orderTitles;

    private List<WorkTimeRecordViewDto> _workTimeRecords;
    private List<WorkOrderViewDto> _workOrders;

    private List<ActivityViewDto> _activities;
    private List<ActivityViewDto> _filteredList;

    private bool _isAddClicked;
    private string _activityId;
    private int _collectionSize;
    private string _orderKey;

    protected override async Task OnInitializedAsync()
    {
        _collectionSize = await DeveloperService.GetCollectionSizeByEmployeeIdAsync(EmployeeId);

        await FetchDataAsync();
    }

    protected override void OnInitialized()
    {
        _workTimeRecords = new();
        _workOrders = new();
        _filteredList = new();
        _activities = new();
        _filters = new() {
            { "Tutti", OnClickOrderByWorkOrder },
            { "Completate", OnClickOrderByName },
            { "Archiviate", OnClickOrderByStartDate },
        };
        _orderTitles = new() {
            { "Cliente", OnClickOrderByClient },
            { "Commessa", OnClickOrderByWorkOrder },
            { "Attività", OnClickOrderByName },
            { "Data d'inizio", OnClickOrderByStartDate },
            { "Data di fine", OnClickOrderByFinishDate },
            { "Ore segnate", OnClickOrderByWorkedHours },
        };
    }

    private void OnClickOrderByClient()
    {
        _filteredList = _activities
            .OrderBy(cl => cl.Name)
            .ToList();

        StateHasChanged();
    }

    private void HandleOrdering(string key)
    {
        if (_filters.ContainsKey(key))
        {
            _filters[key].Invoke();
            _orderKey = key;
        }
    }

    private string SetStyle(string key)
    {
        return key == _orderKey
            ? CssClass.Active
            : CssClass.Empty;
    }

    private void HandleSearchedElements(string query)
    {
        if (string.IsNullOrEmpty(query))
        {
            _filteredList = new(_activities);
        }

        _filteredList = _activities
            .Where(ac =>
                ac.Name
                    .Contains(query, StringComparison.OrdinalIgnoreCase) ||
                ac.WorkOrderName
                    .Contains(query, StringComparison.OrdinalIgnoreCase) ||
                ac.ClientName
                    .Contains(query, StringComparison.OrdinalIgnoreCase)
            )
            .ToList();

        StateHasChanged();
    }

    private async Task FetchDataAsync(int limit = 0, int offset = 5)
    {
        _activities = await DeveloperService.GetActivitiesByEmployeeIdAsync(EmployeeId, limit, offset);

        foreach (var ac in _activities)
        {
            _workTimeRecords.Add(await DeveloperService.GetWorkTimeRecordsAsync(ac.WorkOrderId, ac.Id, EmployeeId));
        }

        _filteredList = new(_activities);
    }

    public void OnClickAddWorkedHours(string activityId)
    {
        _isAddClicked = !_isAddClicked;
        _activityId = activityId;
    }

    private async Task HandlePaginate(int limit, int offset)
    {
        await FetchDataAsync(limit, offset);
    }

    private void OnClickOrderByName()
    {
        _filteredList = _activities
            .OrderBy(ac => ac.Name)
            .ToList();

        StateHasChanged();
    }

    private void OnClickOrderByStartDate()
    {
        _filteredList = _activities
            .OrderBy(ac => ac.StartDate)
            .ToList();

        StateHasChanged();
    }

    private void OnClickOrderByFinishDate()
    {
        _filteredList = _activities
            .OrderBy(ac => ac.FinishDate)
            .ToList();

        StateHasChanged();
    }

    private void OnClickOrderByWorkOrder()
    {
        _filteredList = _activities
            .OrderBy(ac => ac.WorkOrderName)
            .ToList();

        StateHasChanged();
    }

    private void OnClickOrderByWorkedHours()
    {
        _filteredList = _activities
            .Where(ac => _workTimeRecords
                .OrderBy(wtr => wtr.Hours)
                .Any(wtr => wtr.ActivityId == ac.Id)
            )
            .ToList();

        StateHasChanged();
    }
}