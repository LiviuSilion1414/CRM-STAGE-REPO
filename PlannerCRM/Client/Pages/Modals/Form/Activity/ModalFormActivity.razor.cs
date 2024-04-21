using PlannerCRM.Client.Utilities.Navigation;

namespace PlannerCRM.Client.Pages.Modals.Form.Activity;

[Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
public partial class ModalFormActivity : ComponentBase
{
    [Parameter] public string Title { get; set; }
    [Parameter] public string Size { get; set; }
    [Parameter] public ActivityFormDto Model { get; set; }
    [Parameter] public EventCallback<ActivityFormDto> GetValidatedModel { get; set; }

    [Inject] public Logger<ActivityFormDto> Logger { get; set; }
    [Inject] public OperationManagerCrudService OperationManagerService { get; set; }
    [Inject] public NavigationManager NavManager { get; set; }
    [Inject] public NavigationLockService NavigationUtil { get; set; }
    [Inject] public CustomDataAnnotationsValidator CustomValidator { get; set; }

    private Dictionary<string, List<string>> _errors;

    private EditContext _editContext;

    private List<WorkOrderSelectDto> _workOrders;
    private List<EmployeeSelectDto> _employees;

    private readonly bool _isDisabled = true;

    private bool _isError;
    private string _message;
    private bool _isCancelClicked;

    private bool _hideWorkOrdersList;

    private bool _hideEmployeesList;

    private bool _workOrderHasBeenSet;

    protected override void OnInitialized()
    {
        Model = new()
        {
            EmployeeActivity = new(),
            ViewEmployeeActivity = new(),
            DeleteEmployeeActivity = new(),
            SelectedEmployee = string.Empty,
        };
        _editContext = new(Model);
        _workOrders = new();
        _employees = new();
        _hideWorkOrdersList = true;
        _hideEmployeesList = true;
    }

    private async Task HandleSearchedWorkOrders(string query)
    {
        if (string.IsNullOrEmpty(query))
        {
            OnClickInvalidSubmit();
        }
        else
        {
            _workOrders = await OperationManagerService.SearchWorkOrderAsync(query);
        }

        StateHasChanged();
    }

    private async Task HandleSearchedEmployees(string query)
    {
        if (string.IsNullOrEmpty(query))
        {
            OnClickInvalidSubmit();
        }
        else
        {
            _employees = await OperationManagerService.SearchEmployeeAsync(query);
        }

        StateHasChanged();
    }


    private void HandleChosenWorkOrder(WorkOrderSelectDto workOrderSelect)
    {
        Model.WorkOrderId = workOrderSelect.Id;
        Model.SelectedWorkOrder = workOrderSelect.Name;
        Model.ClientName = workOrderSelect.ClientName;
        _workOrderHasBeenSet = !_workOrderHasBeenSet;
    }

    private void OnClickModalCancel() =>
        _isCancelClicked = !_isCancelClicked;

    private void ToggleWorkOrderListView() =>
        _hideWorkOrdersList = !_hideWorkOrdersList;

    private void ToggleEmployeesListView() =>
        _hideEmployeesList = !_hideEmployeesList;

    private void OnClickHideBanner(bool hidden) => _isError = hidden;

    //private void HandleRemoveEmployee(EmployeeActivityDto employeeActivity) {
    //    try {
    //        var isContained = Model.ViewEmployeeActivity
    //            .Any(ea => ea.Employee.FullName == employeeActivity.Employee.FullName);
    //
    //        if (isContained) {
    //            Model.ViewEmployeeActivity.Remove(employeeActivity);
    //        }
    //    } catch (Exception exc) {
    //        Logger.LogError("Error: { } Message: { }", exc.StackTrace, exc.Message);
    //        _message = exc.Message;
    //        _isError = true;
    //    }
    //}

    private void HandleChosenEmployee(EmployeeSelectDto employee)
    {
        try
        {
            var contains = Model.EmployeeActivity
                .Any(ea => ea.Employee.Id == employee.Id);

            if (!contains)
            {
                var activity =
                    new EmployeeActivityDto
                    {
                        EmployeeId = employee.Id,
                        Employee = new EmployeeSelectDto
                        {
                            Id = employee.Id,
                            Email = employee.Email,
                            FirstName = employee.FirstName,
                            LastName = employee.LastName,
                            FullName = employee.FullName,
                            Role = employee.Role,
                            EmployeeActivities = new() {
                                new EmployeeActivityDto() {
                                    EmployeeId = employee.Id,
                                    ActivityId = Model.Id,
                                }
                            }
                        },
                        ActivityId = Model.Id,
                        Activity = new ActivitySelectDto
                        {
                            Id = Model.Id,
                            Name = Model.Name,
                            StartDate = Model?.StartDate
                                ?? throw new NullReferenceException(ExceptionsMessages.NULL_ARG),
                            FinishDate = Model.FinishDate
                                ?? throw new NullReferenceException(ExceptionsMessages.NULL_ARG),
                            WorkOrderId = Model.WorkOrderId
                                ?? throw new NullReferenceException(ExceptionsMessages.NULL_ARG)
                        },

                    };

                Model.EmployeeActivity.Add(activity);
                Model.ViewEmployeeActivity.Add(activity);
            }
            ToggleEmployeesListView();
        }
        catch (NullReferenceException exc)
        {
            Logger.LogError("Error: { } Message: { }", exc.StackTrace, exc.Message);
            _message = exc.Message;
            _isError = true;
        }
        catch (Exception exc)
        {
            Logger.LogError("Error: { } Message: { }", exc.StackTrace, exc.Message);
            _message = exc.Message;
            _isError = true;
        }
    }

    private void OnClickRemoveAsSelected(EmployeeSelectDto employee)
    {
        Model.DeleteEmployeeActivity = AddToDeleteEnumerable(Model.ViewEmployeeActivity, employee);

        RemoveFromEnumerable(Model.EmployeeActivity, employee);
        RemoveFromEnumerable(Model.ViewEmployeeActivity, employee);
    }

    private static void RemoveFromEnumerable(HashSet<EmployeeActivityDto> employeeActivities, EmployeeSelectDto employee)
    {
        employeeActivities
            .Where(ea => ea.EmployeeId == employee.Id)
            .ToList()
            .ForEach(ea => employeeActivities.Remove(ea));
    }

    private static HashSet<EmployeeActivityDto> AddToDeleteEnumerable(HashSet<EmployeeActivityDto> lookupList, EmployeeSelectDto employee)
    {
        return lookupList
            .Where(ea => ea.EmployeeId == employee.Id)
            .ToHashSet();
    }

    private void OnClickInvalidSubmit()
    {
        _isError = true;
        _message = ExceptionsMessages.EMPTY_FIELDS;
    }

    private async Task OnClickModalConfirm()
    {
        var isValid = ValidatorService.Validate(Model, out _errors);

        if (isValid)
        {
            await GetValidatedModel.InvokeAsync(Model);
        }
        else
        {
            CustomValidator.DisplayErrors(_errors);
            OnClickInvalidSubmit();
        }
    }
}