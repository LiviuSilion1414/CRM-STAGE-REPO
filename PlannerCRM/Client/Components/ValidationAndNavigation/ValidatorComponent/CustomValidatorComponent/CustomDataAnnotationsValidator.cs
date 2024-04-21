namespace PlannerCRM.Client.Components.ValidationAndNavigation.ValidatorComponent.CustomValidatorComponent;

public partial class CustomDataAnnotationsValidator : ComponentBase
{
    [CascadingParameter] Dictionary<string, List<string>> Errors { get; set; }
    [CascadingParameter] EditContext CurrentEditContext { get; set; }

    private ValidationMessageStore _messageStore;
    protected override void OnInitialized()
    {
        if (CurrentEditContext is null)
        {
            throw new InvalidOperationException(
                $"{nameof(CustomDataAnnotationsValidator)} " +
                $"requires a cascading parameter of type {nameof(EditContext)}. " +
                $"For example, you can use {nameof(CustomDataAnnotationsValidator)} " +
                $"inside an {nameof(EditForm)}.");
        }

        Errors = new();
        _messageStore = new(CurrentEditContext);

        CurrentEditContext.OnValidationRequested += OnValidationRequested;

        CurrentEditContext.OnFieldChanged += OnFieldChanged;
    }

    private void OnFieldChanged(object sender, FieldChangedEventArgs e)
    {
        _messageStore?.Clear(e.FieldIdentifier);
        Errors.Remove(e.FieldIdentifier.FieldName);
    }

    private void OnValidationRequested(object sender, ValidationRequestedEventArgs e)
        => _messageStore?.Clear();

    public void DisplayErrors(Dictionary<string, List<string>> errors = null)
    {
        if (CurrentEditContext is not null)
        {
            if (errors is not null)
            {
                AddIntoMessageStore(errors);
            }
            else
            {
                AddIntoMessageStore(Errors);
            }

            CurrentEditContext.NotifyValidationStateChanged();
        }
    }

    private void AddIntoMessageStore(Dictionary<string, List<string>> errors)
    {
        foreach (var err in errors)
        {
            foreach (var message in err.Value)
            {
                if (Errors.ContainsKey(err.Key))
                {
                    Errors[err.Key].Add(message);
                }
                else
                {
                    Errors.Add(err.Key, new() { message });
                }

                _messageStore.Add(CurrentEditContext.Field(err.Key), err.Value);
            }
        }
    }

    public void ClearErrors()
    {
        _messageStore?.Clear();
        CurrentEditContext?.NotifyValidationStateChanged();
    }
}