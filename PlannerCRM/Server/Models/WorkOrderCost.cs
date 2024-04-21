namespace PlannerCRM.Server.Models;

public class WorkOrderCost
{
    public string Id { get; set; }
    public string Name { get; set; }
    public int WorkOrderId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime FinishDate { get; set; }
    public DateTime IssuedDate { get; set; }
    public bool IsCreated { get; init; }
    public int TotalHours { get; set; }
    public int TotalEmployees { get; set; }
    public int TotalActivities { get; set; }
    public decimal CostPerMonth { get; set; }
    public decimal TotalCost { get; set; }
    public TimeSpan TotalTime { get; set; }
    public List<Activity> Activities { get; set; }
    public List<Employee> Employees { get; set; }
    public List<ActivityCost> MonthlyActivityCosts { get; set; }
    public int ClientId { get; set; }
}