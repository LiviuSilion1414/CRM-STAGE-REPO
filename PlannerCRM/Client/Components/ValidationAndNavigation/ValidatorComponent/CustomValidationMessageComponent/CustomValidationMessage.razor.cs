namespace PlannerCRM.Client.Components.ValidationAndNavigation.ValidatorComponent.CustomValidationMessageComponent;

public partial class CustomValidationMessage : ComponentBase
{
    [CascadingParameter] Dictionary<string, List<string>> Errors { get; set; }
    [CascadingParameter] EditContext CurrentEditContext { get; set; }
    [Parameter] public string FieldName { get; set; }

    protected override void OnInitialized()
    {
        Errors = new();

        CurrentEditContext.OnFieldChanged += OnFieldChanged;

        CurrentEditContext.NotifyValidationStateChanged();
    }

    private void OnFieldChanged(object sender, FieldChangedEventArgs e)
        => Errors.Remove(e.FieldIdentifier.FieldName);
}