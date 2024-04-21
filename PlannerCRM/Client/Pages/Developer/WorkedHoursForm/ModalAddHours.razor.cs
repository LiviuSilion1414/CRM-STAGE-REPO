using PlannerCRM.Client.Utilities.Navigation;

namespace PlannerCRM.Client.Pages.Developer.WorkedHoursForm;

[Authorize(Roles = nameof(Roles.JUNIOR_DEVELOPER))]
[Authorize(Roles = nameof(Roles.SENIOR_DEVELOPER))]
public partial class ModalAddHours : ComponentBase
{
    [Parameter] public string EmployeeId { get; set; }
    [Parameter] public string ActivityId { get; set; }

    [Inject] public DeveloperService DeveloperService { get; set; }
    [Inject] public AccountManagerCrudService AccountManagerService { get; set; }
    [Inject] public NavigationLockService NavigationUtil { get; set; }
    [Inject] public NavigationManager NavManager { get; set; }
    [Inject] public CustomDataAnnotationsValidator CustomValidator { get; set; }

    private readonly bool _disabled = true;

    private Dictionary<string, List<string>> _errors;

    private WorkTimeRecordFormDto _model;
    private WorkOrderViewDto _workOrder;
    private EditContext _editContext;
    private ActivityViewDto _activity;

    private bool _isError;
    private string _message;

    public bool _isCancelClicked;
    private string _currentPage;

    protected override async Task OnInitializedAsync()
    {
        _model.Employee = await AccountManagerService.GetEmployeeForViewByIdAsync(EmployeeId);
        _model.EmployeeId = _model.Employee.Id;
        _activity = await DeveloperService.GetActivityByIdAsync(ActivityId);
        _workOrder = await DeveloperService.GetWorkOrderByIdAsync(_activity.WorkOrderId);
    }

    protected override void OnInitialized()
    {
        _model = new();
        _workOrder = new();
        _activity = new();
        _errors = new();
        _editContext = new(_model);
        _currentPage = _currentPage = NavigationUtil.GetCurrentPage();
    }

    private void Toggle() => _isCancelClicked = !_isCancelClicked;

    public void OnClickModalCancel() =>
       Toggle();

    public async Task OnClickModalConfirm()
    {
        try
        {
            var isValid = ValidatorService.Validate(_model, out _errors);

            if (isValid)
            {
                _model.Date = DateTime.Now;
                _model.ActivityId = _activity.Id;
                _model.EmployeeId = EmployeeId;
                _model.WorkOrderId = _workOrder.Id;

                var response = await DeveloperService.AddWorkedHoursAsync(_model);

                if (!response.IsSuccessStatusCode)
                {
                    _isError = true;
                    _message = await response.Content.ReadAsstring Async();
                }
                else
                {
                    Toggle();
                    NavManager.NavigateTo(_currentPage, true);
                }
            }
            else
            {
                foreach (var err in _errors)
                {
                    foreach (var item in err.Value)
                    {
                        Console.WriteLine($"Key: {err.Key} Error: {item}");
                    }
                }
                CustomValidator.DisplayErrors(_errors);
                _isError = true;
                _message = ExceptionsMessages.EMPTY_FIELDS;
            }
        }
        catch (Exception exc)
        {
            _isError = true;
            _message = exc.Message;
        }
    }

    private void OnClickHideBanner(bool hidden) => _isError = hidden;
}