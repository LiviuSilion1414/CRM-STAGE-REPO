﻿using PlannerCRM.Shared.Dtos.Entities;

namespace PlannerCRM.Shared.Dtos.JunctionEntities;

public class EmployeeSalaryDto
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public int SalaryId { get; set; }

    // Navigation properties
    public EmployeeDto EmployeeDto { get; set; }
    public SalaryDto SalaryDto { get; set; }
}