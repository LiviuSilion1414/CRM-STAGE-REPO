namespace PlannerCRM.Shared.DTOs.EmployeeDto.Views;

public class EmployeeViewDto
{
    public string Id { get; set; }
    public ProfilePictureDto ProfilePicture { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public Roles Role { get; set; }
    public string NumericCode { get; set; }
    public string Password { get; set; }
    public decimal CurrentHourlyRate { get; set; }
    public DateTime StartDateHourlyRate { get; set; }
    public DateTime FinishDateHourlyRate { get; set; }
    public DateTime BirthDay { get; set; }
    public DateTime StartDate { get; set; }
    public List<EmployeeSalaryDto> EmployeeSalaries { get; set; }
    public List<EmployeeActivityDto> EmployeeActivities { get; set; }
    public string FullName { get; set; }
    public string Name { get; set; }
    public bool IsCompleted { get; set; }
    public bool IsArchived { get; set; }
    public bool IsDeleted { get; set; }
}
