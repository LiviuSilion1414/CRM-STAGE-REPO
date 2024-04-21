namespace PlannerCRM.Shared.DTOs.WorkOrder.Views;

public class WorkOrderViewDto
{
    public DateTime StartDate { get; set; }
    public DateTime FinishDate { get; set; }
    public bool IsInvoiceCreated { get; init; }
    public string ClientId { get; set; }
    public string ClientName { get; init; }
    public string Id { get; set; }
    public string FullName { get; set; }
    public string Name { get; set; }
    public bool IsCompleted { get; set; }
    public bool IsDeleted { get; set; }
    public bool IsArchived { get; set; }
}