namespace PlannerCRM.Server.Models;

public class FirmClient
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string VatNumber { get; set; }

    public List<ClientWorkOrder> WorkOrders { get; set; }
}