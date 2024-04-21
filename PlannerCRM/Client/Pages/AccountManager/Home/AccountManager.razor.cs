namespace PlannerCRM.Client.Pages.AccountManager.Home;

[Authorize(Roles = nameof(Roles.ACCOUNT_MANAGER))]
public partial class AccountManager : ComponentBase
{
    [Inject] public AccountManagerCrudService AccountManagerService { get; set; }

    private List<EmployeeViewDto> _users;
    private List<EmployeeViewDto> _filteredList;
    private Dictionary<string, Action> _filters = new();

    private bool _isAddClicked;
    private int _collectionSize;

    protected override async Task OnInitializedAsync()
    {
        _collectionSize = await AccountManagerService.GetEmployeesSizeAsync();

        await FetchDataAsync();
    }

    protected override void OnInitialized()
    {
        _users = new();
        _filteredList = new();
        _filters = new() {
            { "Tutti",  OnClickAll },
            { "Attivi",  OnClickFilterIfActive },
            { "Archiviati",  OnClickFilterIfArchived },
        };
    }

    private void HandleSearchedElements(string query)
    {
        if (string.IsNullOrEmpty(query))
        {
            _filteredList = new(_users);
        }

        _filteredList = _users
            .Where(us => us.FullName
                .Contains(query, string Comparison.OrdinalIgnoreCase))
            .ToList();

        StateHasChanged();
    }

    private void OnClickAll()
    {
        _filteredList = new(_users);

        StateHasChanged();
    }

    private void OnClickFilterIfActive()
    {
        _filteredList = _users
            .Where(us => !us.IsArchived)
            .ToList();

        StateHasChanged();
    }

    private void OnClickFilterIfArchived()
    {
        _filteredList = _users
            .Where(us => us.IsArchived)
            .ToList();

        StateHasChanged();
    }

    private async Task FetchDataAsync(int limit = 0, int offset = 5)
    {
        _users = await AccountManagerService.GetPaginatedEmployeesAsync(limit, offset);
        _filteredList = new(_users);
    }

    private async Task HandlePaginate(int limit, int offset)
    {
        await FetchDataAsync(limit, offset);
    }

    private void OnClickAddUser() =>
        _isAddClicked = !_isAddClicked;
}