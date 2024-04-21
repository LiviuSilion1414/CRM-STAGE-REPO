namespace PlannerCRM.Client.Components.FilterAndSearch.Search;

public partial class SearchBox : ComponentBase
{
    [Parameter] public EventCallback<string> GetSearchedItems { get; set; }

    private string _query = string.Empty;

    private async Task Search()
        => await GetSearchedItems.InvokeAsync(_query);
}