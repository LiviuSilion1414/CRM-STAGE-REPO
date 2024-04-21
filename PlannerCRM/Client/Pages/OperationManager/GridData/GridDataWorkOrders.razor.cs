namespace PlannerCRM.Client.Pages.OperationManager.GridData;

[Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
public partial class GridDataWorkOrders : ComponentBase
{
    [Parameter] public List<WorkOrderViewDto> WorkOrders { get; set; }

    private Dictionary<string, Action> _orderTitles;
    private WorkOrderViewDto _currentWorkOrder;

    private bool _trIsClicked;

    private bool _isEditWorkOrderClicked;
    private bool _isDeleteWorkOrderClicked;

    private string _workOrderId;
    private string _orderKey;

    protected override void OnInitialized()
    {
        _currentWorkOrder = new();
        _orderTitles = new() {
            { "Stato", OnClickOrderByActive },
            { "Cliente", OnClickOrderByClient },
            { "Nome", OnClickOrderByName },
            { "Data d'inizio", OnClickOrderByStartDate },
            { "Data di fine", OnClickOrderByFinishDate }
        };
    }

    private void HandleOrdering(string key)
    {
        if (_orderTitles.ContainsKey(key))
        {
            _orderTitles[key].Invoke();
            _orderKey = key;
        }
    }

    private void OnClickOrderByActive()
    {
        WorkOrders = WorkOrders
            .OrderBy(wo => !wo.IsArchived || !wo.IsDeleted)
            .ToList();

        StateHasChanged();
    }

    private void OnClickOrderByName()
    {
        WorkOrders = WorkOrders
            .OrderBy(wo => wo.Name)
            .ToList();

        StateHasChanged();
    }

    private void OnClickOrderByClient()
    {
        WorkOrders = WorkOrders
            .OrderBy(wo => wo.ClientName)
            .ToList();

        StateHasChanged();
    }

    private void OnClickOrderByStartDate()
    {
        WorkOrders = WorkOrders
            .OrderBy(wo => wo.StartDate)
            .ToList();

        StateHasChanged();
    }

    private void OnClickOrderByFinishDate()
    {
        WorkOrders = WorkOrders
            .OrderBy(wo => wo.FinishDate)
            .ToList();

        StateHasChanged();
    }

    private void OnClickEditWorkOrder(string id)
    {
        _isEditWorkOrderClicked = !_isEditWorkOrderClicked;
        _workOrderId = id;
    }

    public void OnClickDeleteWorkOrder(string id)
    {
        _isDeleteWorkOrderClicked = !_isDeleteWorkOrderClicked;
        _workOrderId = id;
    }

    private void OnClickTableRow(string workOrderId)
    {
        _trIsClicked = !_trIsClicked;
        _currentWorkOrder = WorkOrders
            .Single(wo => wo.Id == workOrderId);
    }
}