namespace PlannerCRM.Shared.DTOs.CostDto;

public class WorkOrderCostDto
{
    public string WorkOrderId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime FinishDate { get; set; }
    public int TotalHours { get; set; }
    public int TotalEmployees
    { get; set; }
    public int TotalActivities
    { get; set; }
    public decimal CostPerMonth { get; set; }
    public decimal TotalCost { get; set; }
    public TimeSpan TotalTime { get; set; }
    public bool IsInvoiceCreated { get; init; }
    public List<ActivityViewDto> Activities { get; set; }
    public List<EmployeeViewDto> Employees { get; set; }
    public List<ActivityCostDto> MonthlyActivityCosts { get; set; }
    public string ClientId
    { get; set; }
    public string Id { get; set; }
    public string FullName { get; set; }
    public string Name { get; set; }
    public bool IsCompleted { get; set; }
    public bool IsDeleted { get; set; }
    public bool IsArchived { get; set; }
}