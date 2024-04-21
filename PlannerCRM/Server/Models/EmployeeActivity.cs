namespace PlannerCRM.Server.Models;

public class EmployeeActivity
{
    public string Id { get; set; }

    public string EmployeeId { get; set; }
    public Employee Employee { get; set; }

    public string ActivityId { get; set; }
    public Activity Activity { get; set; }
}