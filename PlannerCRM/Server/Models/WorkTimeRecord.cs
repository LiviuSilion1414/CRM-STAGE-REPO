namespace PlannerCRM.Server.Models;

public class WorkTimeRecord
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int Hours { get; set; }
    public Decimal TotalPrice { get; set; }

    public int ActivityId { get; set; }
    public int WorkOrderId { get; set; }

    public int EmployeeId { get; set; }
    public Employee Employee { get; set; }
}