namespace PlannerCRM.Shared.DTOs.ActivityDto.Forms;

public class EmployeeActivityDto
{
    public string Id { get; set; }

    public string EmployeeId { get; set; }
    public EmployeeSelectDto Employee { get; set; }

    public int ActivityId { get; set; }
    public ActivitySelectDto Activity { get; set; }
}
