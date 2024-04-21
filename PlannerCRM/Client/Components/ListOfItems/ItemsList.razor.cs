using System.Linq;

namespace PlannerCRM.Client.Components.ListOfItems;

public partial class ItemsList<T> : ComponentBase
{
    [Parameter] public IEnumerable<T> Items { get; set; }
    [Parameter] public List<string> PropertyKeys { get; set; }
    [Parameter] public EventCallback<T> GetSelectedItem { get; set; }

    private object GetPropertyName(T item)
    {
        var hasMatchingKeys = item
            .GetType()
            .GetProperties()
            .Any(prop => PropertyKeys
                .Any(key => key == prop.Name)
            );

        if (hasMatchingKeys)
        {
            return item
                .GetType()
                .GetProperty(PropertyKeys.First())
                .GetValue(item);
        }

        return "name";
    }

    private async Task SetAsSelected(T optionItem)
    {
        if (optionItem is not null)
        {
            await GetSelectedItem.InvokeAsync(optionItem);
        }
    }
}