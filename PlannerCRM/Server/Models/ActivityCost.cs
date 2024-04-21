namespace PlannerCRM.Server.Models;

public class ActivityCost
{
    public string Id { get; set; }
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime FinishDate { get; set; }
    public List<Employee> Employees { get; set; }
    public decimal MonthlyCost { get; set; }
}