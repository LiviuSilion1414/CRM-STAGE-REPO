namespace PlannerCRM.Shared.DTOs.CostDto;

public class EmployeeCostDto
{
    public string Id { get; set; }
    public string Email { get; set; }
    public string FullName { get; set; }
    public decimal CurrentHourlyRate { get; set; }
    public List<EmployeeSalaryDto> Salaries { get; set; }
    public List<EmployeeActivityDto> Activities { get; set; }
}