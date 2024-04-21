namespace PlannerCRM.Shared.DTOs.WorkTimeDto.Views;

public class WorkTimeRecordViewDto
{
    public string Id { get; set; }
    public DateTime Date { get; set; }
    public int Hours { get; set; }
    public decimal TotalPrice { get; set; }

    public string ActivityId { get; set; }
    public string WorkOrderId
    { get; set; }

    public string EmployeeId { get; set; }
}