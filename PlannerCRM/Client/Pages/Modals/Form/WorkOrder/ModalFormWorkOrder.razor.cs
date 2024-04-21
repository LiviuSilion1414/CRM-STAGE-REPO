using PlannerCRM.Client.Utilities.Navigation;

namespace PlannerCRM.Client.Pages.Modals.Form.WorkOrder;

[Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
public partial class ModalFormWorkOrder : ComponentBase
{
    [Parameter] public string Title { get; set; }
    [Parameter] public WorkOrderFormDto Model { get; set; }
    [Parameter] public EventCallback<WorkOrderFormDto> GetValidatedModel { get; set; }

    [Inject] public NavigationLockService NavigationUtil { get; set; }
    [Inject] public OperationManagerCrudService OperationManagerService { get; set; }
    [Inject] public CustomDataAnnotationsValidator CustomValidator { get; set; }
    [Inject] public NavigationManager NavManager { get; set; }
    [Inject] public Logger<WorkOrderFormDto> Logger { get; set; }

    private List<ClientViewDto> _clients;

    private Dictionary<string, List<string>> _errors;
    private EditContext _editContext;

    private string _currentPage;
    private string _errorMessage;
    private bool _isError;
    private bool _isCancelClicked;
    private bool _isClientSelected;

    private bool _hideclientsList;

    protected override void OnInitialized()
    {
        Model = new();
        _editContext = new(Model);
        _clients = new();
        CustomValidator = new();
        _isCancelClicked = false;
        _currentPage = _currentPage = NavigationUtil.GetCurrentPage();
    }

    private void OnClickModalCancel()
    {
        _isCancelClicked = !_isCancelClicked;
        NavManager.NavigateTo(_currentPage);
    }

    private void OnClickHideBanner(bool hidden) => _isError = hidden;

    private async Task HandleSearchedElements(string query)
    {
        if (string.IsNullOrEmpty(query))
        {
            OnClickInvalidSubmit();
        }
        else
        {
            _clients = await OperationManagerService.SearchClientAsync(query);
        }

        StateHasChanged();
    }

    private void OnClickInvalidSubmit()
    {
        _isError = true;
        _errorMessage = ExceptionsMessages.EMPTY_FIELDS;
    }

    private void OnClickSetAsClient(ClientViewDto client)
    {
        _isClientSelected = !_isClientSelected;
        Model.ClientId = client.Id;
        Model.ClientName = client.Name;
        _hideclientsList = !_hideclientsList;
    }

    private async Task OnClickModalConfirm()
    {
        try
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
