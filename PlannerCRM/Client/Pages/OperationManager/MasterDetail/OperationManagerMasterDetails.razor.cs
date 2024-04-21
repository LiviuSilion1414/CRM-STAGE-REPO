namespace PlannerCRM.Client.Pages.OperationManager.MasterDetail;

[Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
public partial class OperationManagerMasterDetails : ComponentBase
{
    [Parameter] public string WorkOrderId { get; set; }
    [Parameter] public WorkOrderViewDto WorkOrder { get; set; }

    [Inject] public OperationManagerCrudService OperationManagerService { get; set; }

    private WorkOrderViewDto _workOrder;
    private List<ActivityViewDto> _activities;

    private bool _isEditActivityClicked;
    private bool _isDeleteActivityClicked;
    private bool _isShowActivityClicked;
    private string _activityId;
    private ActivityViewDto _activityView;

    protected override async Task OnInitializedAsync()
    {
        _workOrder = await OperationManagerService.GetWorkOrderForViewByIdAsync(WorkOrderId);
        _activities = await OperationManagerService.GetActivityByWorkOrderAsync(_workOrder.Id);
    }

    protected override void OnInitialized()
    {
        _workOrder = new();
        _activities = new();
    }

    private void OnClickEdit(string activityId)
    {
        _isEditActivityClicked = !_isEditActivityClicked;
        _activityId = activityId;
    }

    private void OnClickShowDetails(ActivityViewDto activity)
    {
        _isShowActivityClicked = !_isShowActivityClicked;
        _activityView = new()
        {
            Id = activity.Id,
            Name = activity.Name,
            StartDate = activity.StartDate,
            FinishDate = activity.FinishDate,
            WorkOrderId = activity.WorkOrderId,
            EmployeeActivity = activity.EmployeeActivity
        };
    }

    private void OnClickDelete(string activityId)
    {
        _isDeleteActivityClicked = !_isDeleteActivityClicked;
        _activityId = activityId;
    }
}