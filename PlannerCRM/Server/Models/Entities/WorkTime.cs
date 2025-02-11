﻿namespace PlannerCRM.Server.Models.Entities;

public class WorkTime
{
    public int Id { get; set; }
    public DateTime CreationDate { get; set; }

    public double WorkedHours { get; set; }
    
    // Navigation properties
    public int WorkOrderId { get; set; }
    public int EmployeeId { get; set; }
    public int ActivityId { get; set; }
    public WorkOrder WorkOrder { get; set; }
    public Employee Employee { get; set; }
    public Activity Activity { get; set; }
    public List<ActivityWorkTime> ActivityWorkTimes { get; set; }
}
