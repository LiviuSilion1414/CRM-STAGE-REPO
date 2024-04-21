namespace PlannerCRM.Server.Services;

public class CalculatorService
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<CalculatorService> _logger;

    public CalculatorService(AppDbContext dbContext, ILogger<CalculatorService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<int> GetCollectionSizeAsync()
        => await _dbContext.WorkOrderCosts
            .CountAsync();

    public async Task AddInvoiceAsync(string workOrderId)
    {
        try
        {
            var isAnyWorkOrder = await _dbContext.WorkOrders
                .AnyAsync(wo => wo.Id == workOrderId);

            var isAnyInvoice = await _dbContext.WorkOrderCosts
                .AnyAsync(inv => inv.WorkOrderId == workOrderId);

            if (!isAnyWorkOrder)
            {
                throw new KeyNotFoundException(ExceptionsMessages.WORKORDER_NOT_FOUND);
            }

            if (isAnyInvoice)
            {
                throw new DuplicateElementException(ExceptionsMessages.DUPLICATE_OBJECT);
            }

            var workOrder = await _dbContext.WorkOrders
                .SingleAsync(wo => wo.Id == workOrderId);

            workOrder.IsCompleted = true;

            _dbContext.Update(workOrder);

            var workOrderCost = await ExecuteCalculationsAsync(workOrder, true);

            await _dbContext.WorkOrderCosts.AddAsync(workOrderCost);

            if (await _dbContext.SaveChangesAsync() == 0)
            {
                throw new DbUpdateException(ExceptionsMessages.IMPOSSIBLE_SAVE_CHANGES);
            }
        }
        catch (Exception exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            throw;
        }
    }

    public async Task<List<WorkOrderViewDto>> GetPaginatedWorkOrdersCostsAsync(int limit, int offset)
    {
        return await _dbContext.WorkOrders
            .OrderBy(wo => wo.Id)
            .Skip(limit)
            .Take(offset)
            .Select(wo =>
                new WorkOrderViewDto
                {
                    Id = wo.Id,
                    Name = wo.Name,
                    StartDate = wo.StartDate,
                    FinishDate = wo.FinishDate,
                    IsCompleted = wo.IsDeleted,
                    IsDeleted = wo.IsDeleted,
                    IsInvoiceCreated = wo.IsInvoiceCreated,
                    ClientId = wo.ClientId,
                    ClientName = _dbContext.Clients
                        .Single(cl => cl.Id == wo.ClientId)
                        .Name
                }
            )
            .ToListAsync();
    }

    public async Task<WorkOrderCostDto> GetWorkOrderCostForViewByIdAsync(string workOrderId)
    {
        try
        {
            if (string.IsNullOrEmpty(workOrderId))
            {
                throw new KeyNotFoundException(ExceptionsMessages.WORKORDER_NOT_FOUND);
            }

            var workOrder = await _dbContext.WorkOrders
                .SingleAsync(wo => wo.Id == workOrderId);
            var workOrderCost = await ExecuteCalculationsAsync(workOrder);

            var employees = workOrderCost.Employees
                    .Select(em =>
                        new EmployeeViewDto
                        {
                            Id = em.Id,
                            FirstName = em.FirstName,
                            LastName = em.LastName,
                            FullName = $"{em.FirstName} {em.LastName}",
                            BirthDay = em.BirthDay,
                            StartDate = em.StartDate,
                            NumericCode = em.NumericCode,
                            Password = em.Password,
                            Email = em.Email,
                            IsDeleted = em.IsDeleted,
                            IsArchived = em.IsArchived,
                            Role = em.Role,
                            CurrentHourlyRate = em.CurrentHourlyRate,
                            EmployeeSalaries = new(),
                            EmployeeActivities = new()
                        }
                    )
                    .ToList();

            var activities = workOrderCost.Activities
                    .Select(ac =>
                        new ActivityViewDto
                        {
                            Id = ac.Id,
                            Name = ac.Name,
                            StartDate = ac.StartDate,
                            FinishDate = ac.FinishDate,
                            WorkOrderId = ac.WorkOrderId,
                            EmployeeActivity = new()
                        }
                    )
                    .ToList();

            var monthlyActivityCosts = workOrderCost.MonthlyActivityCosts
                    .Select(ac =>
                        new ActivityCostDto
                        {
                            Id = ac.Id,
                            Name = ac.Name,
                            StartDate = ac.StartDate,
                            FinishDate = ac.FinishDate,
                            MonthlyCost = ac.MonthlyCost,
                            Employees = employees
                        }
                    )
                    .ToList();

            return new WorkOrderCostDto
            {
                Id = workOrderCost.Id,
                WorkOrderId = workOrderCost.WorkOrderId,
                Name = workOrderCost.Name,
                StartDate = workOrderCost.StartDate,
                FinishDate = workOrderCost.FinishDate,
                TotalTime = workOrderCost.FinishDate - workOrderCost.StartDate,
                ClientId = workOrderCost.ClientId,
                IsInvoiceCreated = workOrderCost.IsCreated,
                Employees = employees,
                Activities = activities,
                MonthlyActivityCosts = monthlyActivityCosts,
                TotalEmployees = workOrderCost.TotalEmployees,
                TotalActivities = workOrderCost.TotalActivities,
                TotalHours = workOrderCost.TotalHours,
                TotalCost = workOrderCost.TotalCost,
                CostPerMonth = workOrderCost.CostPerMonth
            };
        }
        catch (Exception exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            throw;
        }
    }

    private async Task SetInvoiceCreatedFlagAsync(WorkOrder workOrder)
    {
        workOrder.IsInvoiceCreated = true;

        _dbContext.Update(workOrder);

        if (await _dbContext.SaveChangesAsync() == 0)
        {
            throw new DbUpdateException(ExceptionsMessages.IMPOSSIBLE_SAVE_CHANGES);
        }
    }

    private async Task<WorkOrderCost> ExecuteCalculationsAsync(WorkOrder workOrder, bool onCreate = false)
    {
        try
        {
            if (onCreate)
            {
                await SetInvoiceCreatedFlagAsync(workOrder);
            }

            var employees = await GetAllRelatedEmployeesAsync(workOrder.Id);
            var activities = await GetAllRelatedActivitiesAsync(workOrder.Id);
            var monthlyActivityCosts = await CalculateMonthlyCostAsync(workOrder.Id);
            var totalEmployees = await GetRelatedEmployeesSizeAsync(workOrder.Id);
            var totalActivities = await GetRelatedActivitiesSizeAsync(workOrder.Id);
            var totalHours = await CalculateTotalHoursAsync(workOrder.Id);
            var totalCost = (await CalculateMonthlyCostAsync(workOrder.Id))
                .Sum(cost => cost.MonthlyCost);
            var monthlyCost = (await CalculateMonthlyCostAsync(workOrder.Id))
                .Sum(cost => cost.MonthlyCost);
            var activitiesSize = await GetRelatedActivitiesSizeAsync(workOrder.Id);
            var costPerMonth = activitiesSize == 0
                ? 0
                : monthlyCost / activitiesSize;

            return new WorkOrderCost
            {
                WorkOrderId = workOrder.Id,
                Name = workOrder.Name,
                StartDate = workOrder.StartDate,
                FinishDate = workOrder.FinishDate,
                TotalTime = workOrder.FinishDate - workOrder.StartDate,
                IsCreated = true,
                IssuedDate = DateTime.Now,
                ClientId = workOrder.ClientId,
                Employees = employees,
                Activities = activities,
                MonthlyActivityCosts = monthlyActivityCosts,
                TotalEmployees = totalEmployees,
                TotalActivities = totalActivities,
                TotalHours = totalHours,
                TotalCost = totalCost,
                CostPerMonth = costPerMonth
            };
        }
        catch (Exception exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            throw;
        }
    }

    private async Task<int> CalculateTotalHoursAsync(string workOrderId)
    {
        return await _dbContext.WorkTimeRecords
            .Where(wtr => wtr.WorkOrderId == workOrderId)
            .SumAsync(x => x.Hours);
    }

    private async Task<int> GetRelatedEmployeesSizeAsync(string workOrderId)
    {
        return await _dbContext.Employees
            .Where(em => _dbContext.WorkTimeRecords
                .Any(wtr => wtr.EmployeeId == em.Id && wtr.WorkOrderId == workOrderId))
            .CountAsync();
    }

    private async Task<int> GetRelatedActivitiesSizeAsync(string workOrderId)
    {
        return await _dbContext.Activities
            .Where(ac => ac.WorkOrderId == workOrderId)
            .CountAsync();
    }

    private async Task<List<Employee>> GetAllRelatedEmployeesAsync(string workOrderId)
    {
        return await _dbContext.Employees
            .Where(em => _dbContext.EmployeeActivity
                .Any(ea => em.Id == ea.EmployeeId))
            .ToListAsync();
    }

    private async Task<List<Activity>> GetAllRelatedActivitiesAsync(string workOrderId)
    {
        return await _dbContext.Activities
            .Where(ac => ac.WorkOrderId == workOrderId)
            .ToListAsync();
    }

    private async Task<List<ActivityCost>> CalculateMonthlyCostAsync(string workOrderId)
    {
        var workTimeRecords = await _dbContext.WorkTimeRecords
            .Where(wtr => wtr.WorkOrderId == workOrderId)
            .ToListAsync();

        var activities = await _dbContext.Activities
            .Where(ac => ac.WorkOrderId == workOrderId)
            .ToListAsync();

        var salaries = await _dbContext.Employees
            .Where(em => em.Salaries
                .Any(sal => sal.EmployeeId == em.Id) &&
                _dbContext.WorkTimeRecords
                    .Any(wtr => wtr.EmployeeId == em.Id))
            .Select(em =>
                new EmployeeSalary
                {
                    EmployeeId = em.Id,
                    StartDate = em.Salaries
                        .Single(sal => sal.EmployeeId == em.Id)
                        .StartDate,
                    FinishDate = em.Salaries
                        .Single(sal => sal.EmployeeId == em.Id)
                        .FinishDate,
                    Salary = em.Salaries
                        .Single(sal => sal.EmployeeId == em.Id)
                        .Salary
                }
            )
            .ToListAsync();

        var output = workTimeRecords
            .Where(wtr => salaries
                .Any(sal => sal.EmployeeId == wtr.EmployeeId))
            .Select(activityCost =>
                new ActivityCost
                {
                    Id = activityCost.Id,
                    Name = activities
                        .First(ac => ac.WorkOrderId == activityCost.WorkOrderId)
                        .Name,
                    StartDate = activities
                        .First(ac => ac.WorkOrderId == activityCost.WorkOrderId)
                        .StartDate,
                    FinishDate = activities
                        .First(ac => ac.WorkOrderId == activityCost.WorkOrderId)
                        .FinishDate,
                    Employees = _dbContext.Employees
                        .Where(em => em.Id == activityCost.EmployeeId)
                        .ToList(),
                    MonthlyCost = salaries
                        .First(ems => ems.EmployeeId == activityCost.EmployeeId)
                        .Salary * activityCost.Hours
                }
            )
            .ToList();

        return output;
    }
}