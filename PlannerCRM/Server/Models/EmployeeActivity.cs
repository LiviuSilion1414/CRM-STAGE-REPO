namespace PlannerCRM.Server.Models;

public class EmployeeActivity
{
    public int Id { get; set; }
    
    public string EmployeeId { get; set; }
    public Employee Employee { get; set; }

    public int ActivityId { get; set; }
    public Activity Activity { get; set; }
}