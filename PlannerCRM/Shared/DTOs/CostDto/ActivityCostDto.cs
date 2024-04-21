namespace PlannerCRM.Shared.DTOs.CostDto;

public class ActivityCostDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime FinishDate { get; set; }
    public List<EmployeeViewDto> Employees { get; set; }
    public decimal MonthlyCost { get; set; }
}