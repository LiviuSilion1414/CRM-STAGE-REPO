namespace PlannerCRM.Client.Components.FilterAndSearch.Filter;

public partial class OptionsFilter : ComponentBase
{
    [Parameter] public Dictionary<string, Action> Actions { get; set; }

    private string _filterKey;

    protected override void OnInitialized()
    {
        _filterKey = Actions.Any()
            ? Actions.Keys.First()
            : string.Empty;
    }

    private void HandleFiltering(string key)
    {
        if (Actions.ContainsKey(key))
        {
            Actions[key].Invoke();

            _filterKey = key;
        }
    }

    private string SetStyle(string key)
    {
        return key == _filterKey
            ? CssClass.Selected
            : CssClass.Empty;
    }
}