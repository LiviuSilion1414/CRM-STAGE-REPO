﻿namespace PlannerCRM.Server.Models.Entities;

public class FirmClient
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string VatNumber { get; set; }

    // Navigation properties
    public List<WorkOrder> WorkOrders { get; set; }
    public List<WorkOrderCost> WorkOrderCosts { get; set; }
}
