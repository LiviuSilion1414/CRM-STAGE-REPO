namespace PlannerCRM.Client.Pages.Modals.Form.User;

[Authorize(Roles = nameof(Roles.ACCOUNT_MANAGER))]
public partial class ModalFormUser : ComponentBase
{
    [Parameter] public string Title { get; set; }
    [Parameter] public EmployeeFormDto Model { get; set; }
    [Parameter] public OperationType OperationType { get; set; }
    [Parameter] public EventCallback<EmployeeFormDto> GetValidatedModel { get; set; }

    [Inject] public AccountManagerCrudService AccountManagerService { get; set; }
    [Inject] public CustomDataAnnotationsValidator CustomValidator { get; set; }
    [Inject] public NavigationLockService NavigationUtil { get; set; }
    [Inject] public NavigationManager NavManager { get; set; }
    [Inject] public Base64Converter Converter { get; set; }
    [Inject] public ILogger<EmployeeFormDto> Logger { get; set; }

    private Dictionary<string, List<string>> _errors;

    private EditContext _editContext;
    private string _currentPage;

    private string _errorMessage;
    private bool _isError;
    private bool _isCancelClicked;
    private string _input;

    protected override void OnInitialized()
    {
        Model = new()
        {
            ProfilePicture = new()
            {
                EmployeeInfo = new()
                {
                    EmployeeActivities = new(),
                    EmployeeSalaries = new()
                }
            }
        };
        _editContext = new(Model);
        CustomValidator = new();
        _isCancelClicked = false;
        _currentPage = _currentPage = NavigationUtil.GetCurrentPage();
    }

    private void SwitchPassword(string type) => _input = type;

    private void OnClickModalCancel()
    {
        _isCancelClicked = !_isCancelClicked;
        NavManager.NavigateTo(_currentPage);
    }

    private void OnClickHideBanner(bool hidden) =>
        _isError = hidden;

    private void OnClickInvalidSubmit()
    {
        _isError = true;
        _errorMessage = ExceptionsMessages.EMPTY_FIELDS;
    }

    private async Task SaveImage(InputFileChangeEventArgs args)
    {
        var (thumbnail, imageType) = await Converter.ConvertImageAsync(args);

        Model.ProfilePicture.Thumbnail = thumbnail;
        Model.ProfilePicture.ImageType = imageType;
    }

    private async Task OnClickModalConfirm()
    {
        try
        {
            var isValid = ValidatorService.Validate(Model, out _errors);

            if (isValid)
            {

                if (OperationType == OperationType.ADD)
                {
                    Model.Id = string.Empty;
                    Model.OldEmail = string.Empty;
                }

                Model.ProfilePicture.EmployeeInfo.FirstName = Model.FirstName;
                Model.ProfilePicture.EmployeeInfo.LastName = Model.LastName;
                Model.ProfilePicture.EmployeeInfo.FullName = $"{Model.FirstName} {Model.LastName}";
                Model.ProfilePicture.EmployeeInfo.Email = Model.Email;
                Model.ProfilePicture.EmployeeInfo.Id = Model.Id;
                Model.ProfilePicture.EmployeeInfo.Role = Model.Role ?? throw new ArgumentNullException(ExceptionsMessages.NULL_ARG, nameof(Model.Role));

                Model.EmployeeSalaries = new() {
                    new() {
                        Id = string .Empty,
                        EmployeeId = string .Empty,
                        Salary = Model.CurrentHourlyRate,
                        StartDate = Model.StartDateHourlyRate
                            ?? throw new NullReferenceException(ExceptionsMessages.NULL_ARG),
                        FinishDate = Model.FinishDateHourlyRate
                            ?? throw new NullReferenceException(ExceptionsMessages.NULL_ARG)
                    }
                };

                await GetValidatedModel.InvokeAsync(Model);
            }
            else
            {
                CustomValidator.DisplayErrors(_errors);
                OnClickInvalidSubmit();
            }
        }
        catch (NullReferenceException exc)
        {
            Logger.LogError("Error: { } Message: { }", exc.StackTrace, exc.Message);
            _errorMessage = exc.Message;
            _isError = true;
        }
        catch (Exception exc)
        {
            Logger.LogError("Error: { } _message: { }", exc.StackTrace, exc.Message);
            _errorMessage = exc.Message;
            _isError = true;
        }
    }
}