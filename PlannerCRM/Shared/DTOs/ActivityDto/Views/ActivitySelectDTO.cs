namespace PlannerCRM.Shared.DTOs.ActivityDto.Views;

public class ActivitySelectDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime FinishDate { get; set; }

    public int WorkOrderId { get; set; }
}