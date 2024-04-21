namespace PlannerCRM.Shared.DTOs.ActivityDto.Views;

public class ActivityDeleteDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime FinishDate { get; set; }
    public string WorkOrderId { get; set; }

    public HashSet<EmployeeSelectDto> Employees { get; set; }
}