namespace PlannerCRM.Client.Components.ToggleView;

public partial class SwitchInputType : ComponentBase
{
    [Parameter] public EventCallback<string> Switch { get; set; }

    private bool _isSelected;
    private string _input;

    protected override async Task OnInitializedAsync()
    {
        _isSelected = false;
        _input = Types.Password;

        await Switch.InvokeAsync(_input);
    }

    private async Task Toggle()
    {
        _isSelected = !_isSelected;

        _input = _isSelected ? Types.Text : Types.Password;

        await Switch.InvokeAsync(_input);
    }

    internal class Types
    {
        public const string Text = "text";
        public const string Password = "password";
    }
}