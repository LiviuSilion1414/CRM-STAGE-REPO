namespace PlannerCRM.Shared.DTOs.WorkOrder.Views;

public class WorkOrderDeleteDto
{
	public string Id { get; set; }
	public string Name { get; set; }
	public DateTime StartDate { get; set; }
	public DateTime FinishDate { get; set; }
	public string ClientName { get; set; }
}