namespace PlannerCRM.Shared.Dtos.ClientDto;

public class ClientViewDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string VatNumber { get; set; }
    public int WorkOrderId { get; set; }
}