namespace PlannerCRM.Server.Models;

public class WorkOrder
{
    public string Id { get; set; }
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime FinishDate { get; set; }
    public bool IsInvoiceCreated { get; set; }
    public bool IsCompleted { get; set; }
    public bool IsDeleted { get; set; }
    public List<Activity> Activities { get; set; }
    public List<WorkTimeRecord> WorkTimeRecords { get; set; }
    public int ClientId { get; set; }
    public FirmClient Client { get; set; }
}


