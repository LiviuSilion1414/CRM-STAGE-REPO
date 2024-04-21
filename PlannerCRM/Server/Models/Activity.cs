namespace PlannerCRM.Server.Models;

public class Activity
{
    public string Id { get; set; }
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime FinishDate { get; set; }
    public bool IsDeleted { get; set; }

    public HashSet<EmployeeActivity> EmployeeActivity { get; set; }
    public string WorkOrderId { get; set; }
}