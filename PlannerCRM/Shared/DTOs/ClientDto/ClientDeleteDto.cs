namespace PlannerCRM.Shared.DTOs.ClientDto;

public class ClientDeleteDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string VatNumber { get; set; }
    public int WorkOrderId { get; set; }
}