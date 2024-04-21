namespace PlannerCRM.Server.Repositories;

public class ActivityRepository
{
    private readonly AppDbContext _dbContext;
    private readonly DtoValidatorUtillity _validator;
    private readonly ILogger<DtoValidatorUtillity> _logger;

    public ActivityRepository(
            AppDbContext dbContext,
            DtoValidatorUtillity validator,
            Logger<DtoValidatorUtillity> logger)
    {
        _dbContext = dbContext;
        _validator = validator;
        _logger = logger;
    }

    public async Task AddAsync(ActivityFormDto dto)
    {
        try
        {
            var isValid = await _validator.ValidateActivityAsync(dto, OperationType.ADD);

            if (isValid)
            {
                var model = new Activity
                {
                    Id = dto.Id,
                    Name = dto.Name,
                    StartDate = dto.StartDate
                        ?? throw new NullReferenceException(ExceptionsMessages.NULL_ARG),
                    FinishDate = dto.FinishDate
                        ?? throw new NullReferenceException(ExceptionsMessages.NULL_ARG),
                    WorkOrderId = dto.WorkOrderId
                        ?? throw new NullReferenceException(ExceptionsMessages.NULL_ARG),
                    EmployeeActivity = dto.EmployeeActivity
                        .Select(ea => new EmployeeActivity
                        {
                            Id = ea.Id,
                            EmployeeId = ea.EmployeeId,
                            ActivityId = dto.Id
                        })
                        .ToHashSet()
                };

                await _dbContext.Activities.AddAsync(model);

                var workOrder = await _dbContext.WorkOrders
                    .SingleAsync(wo => wo.Id == dto.WorkOrderId);

                workOrder.Activities.Add(model);

                _dbContext.Update(workOrder);

                if (await _dbContext.SaveChangesAsync() == 0)
                {
                    throw new DbUpdateException(ExceptionsMessages.IMPOSSIBLE_SAVE_CHANGES);
                }
            }
            else
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

    public async Task DeleteAsync(string id)
    {
        try
        {
            var activityDelete = await _validator.ValidateDeleteActivityAsync(id);

            await _dbContext.EmployeeActivity
                .Where(ea => ea.ActivityId == activityDelete.Id)
                .ForEachAsync(ea =>
                    _dbContext.EmployeeActivity
                        .Remove(ea)
                );

            _dbContext.Activities.Remove(activityDelete);

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

    public async Task EditAsync(ActivityFormDto dto)
    {
        try
        {
            var isValid = await _validator.ValidateActivityAsync(dto, OperationType.EDIT);

            if (isValid)
            {
                var model = await _dbContext.Activities
                    .SingleAsync(ac => ac.Id == dto.Id);

                model.Id = dto.Id;
                model.Name = dto.Name;
                model.StartDate = dto.StartDate
                    ?? throw new NullReferenceException(ExceptionsMessages.NULL_ARG);
                model.FinishDate = dto.FinishDate
                    ?? throw new NullReferenceException(ExceptionsMessages.NULL_ARG);
                model.WorkOrderId = dto.WorkOrderId
                    ?? throw new NullReferenceException(ExceptionsMessages.NULL_ARG);
                model.EmployeeActivity = dto.EmployeeActivity
                    .Select(ea => new EmployeeActivity
                    {
                        EmployeeId = ea.EmployeeId,
                        ActivityId = ea.ActivityId,
                    })
                    .ToHashSet();

                var employeesToRemove = dto.DeleteEmployeeActivity
                    .Where(eaDto => _dbContext.EmployeeActivity
                        .Any(ea => eaDto.EmployeeId == ea.EmployeeId))
                    .Select(e =>
                        new EmployeeActivity()
                        {
                            Id = e.Id,
                            EmployeeId = e.EmployeeId,
                            ActivityId = dto.Id
                        }
                    )
                    .ToList();

                employeesToRemove
                    .ForEach(item => _dbContext.EmployeeActivity.Remove(item));

                var workOrder = await _dbContext.WorkOrders
                    .SingleAsync(wo => wo.Id == dto.WorkOrderId);

                var activity = workOrder.Activities
                    .Single(ac => ac.Id == dto.Id);

                activity.Id = dto.Id;
                activity.Name = dto.Name;
                activity.StartDate = dto.StartDate
                    ?? throw new NullReferenceException(ExceptionsMessages.NULL_ARG);
                activity.FinishDate = dto.FinishDate
                    ?? throw new NullReferenceException(ExceptionsMessages.NULL_ARG);
                activity.WorkOrderId = dto.WorkOrderId
                    ?? throw new NullReferenceException(ExceptionsMessages.NULL_ARG);
                activity.EmployeeActivity = dto.EmployeeActivity
                    .Select(ea => new EmployeeActivity
                    {
                        Id = ea.Id,
                        EmployeeId = ea.EmployeeId,
                        ActivityId = ea.ActivityId,
                    }).ToHashSet();

                _dbContext.Update(model);
                _dbContext.Update(workOrder);

                if (await _dbContext.SaveChangesAsync() == 0)
                {
                    throw new DbUpdateException(ExceptionsMessages.IMPOSSIBLE_SAVE_CHANGES);
                }

            }
        }
        catch (Exception exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            throw;
        }
    }

    public async Task<ActivityViewDto> GetForViewByIdAsync(string id)
    {
        return await _dbContext.Activities
            .Select(ac => new ActivityViewDto
            {
                Id = ac.Id,
                Name = ac.Name,
                StartDate = ac.StartDate,
                FinishDate = ac.FinishDate,
                WorkOrderId = ac.WorkOrderId,
                EmployeeActivity = ac.EmployeeActivity
                    .Select(ea => new EmployeeActivityDto
                    {
                        Id = ea.Id,
                        EmployeeId = ea.EmployeeId,
                        Employee = _dbContext.Employees
                            .Select(em => new EmployeeSelectDto
                            {
                                Id = ea.EmployeeId,
                                Email = ea.Employee.Email,
                                FirstName = ea.Employee.FirstName,
                                LastName = ea.Employee.LastName,
                                FullName = ea.Employee.FullName,
                                Role = ea.Employee.Role,
                                CurrentHourlyRate = ea.Employee.CurrentHourlyRate,
                                EmployeeSalaries = ea.Employee.Salaries
                                    .Select(sal => new EmployeeSalaryDto
                                    {
                                        Id = sal.Id,
                                        EmployeeId = ea.EmployeeId,
                                        StartDate = sal.StartDate,
                                        FinishDate = sal.FinishDate,
                                        Salary = sal.Salary,
                                    }).ToList()
                            })
                            .Single(em => em.Id == ea.EmployeeId),
                        ActivityId = ea.ActivityId,
                        Activity = _dbContext.Activities
                            .Select(a => new ActivitySelectDto
                            {
                                Id = ea.ActivityId,
                                Name = ea.Activity.Name,
                                StartDate = ea.Activity.StartDate,
                                FinishDate = ea.Activity.FinishDate,
                                WorkOrderId = ea.Activity.WorkOrderId
                            })
                            .Single(ac => ac.Id == ea.ActivityId)
                    }).ToHashSet()
            })
            .SingleAsync(ac => ac.Id == id);
    }

    public async Task<ActivityFormDto> GetForEditByIdAsync(string activityId)
    {
        return await _dbContext.Activities
            .Where(ac => ac.Id == activityId &&
                _dbContext.WorkOrders
                    .Any(wo => wo.Id == ac.WorkOrderId && !wo.IsDeleted || !wo.IsInvoiceCreated))
            .Select(ac => new ActivityFormDto
            {
                Id = ac.Id,
                Name = ac.Name,
                StartDate = ac.StartDate,
                FinishDate = ac.FinishDate,
                WorkOrderId = ac.WorkOrderId,
                //ClientName = _dbContext.Clients
                //    .Single(cl => cl.WorkOrdersIds.Any(id => id == ac.WorkOrderId)
                //    .Name,
                EmployeeActivity = new(),
                ViewEmployeeActivity = ac.EmployeeActivity
                    .Select(ea => new EmployeeActivityDto
                    {
                        Id = ea.Id,
                        EmployeeId = ea.EmployeeId,
                        Employee = _dbContext.Employees
                            .Select(em => new EmployeeSelectDto
                            {
                                Id = ea.EmployeeId,
                                Email = ea.Employee.Email,
                                FirstName = ea.Employee.FirstName,
                                LastName = ea.Employee.LastName,
                                FullName = ea.Employee.FullName,
                                Role = ea.Employee.Role,
                                CurrentHourlyRate = ea.Employee.CurrentHourlyRate,
                                EmployeeSalaries = ea.Employee.Salaries
                                    .Select(sal => new EmployeeSalaryDto
                                    {
                                        Id = sal.Id,
                                        EmployeeId = ea.EmployeeId,
                                        StartDate = sal.StartDate,
                                        FinishDate = sal.FinishDate,
                                        Salary = sal.Salary,
                                    }).ToList()
                            })
                            .Single(em => em.Id == ea.EmployeeId),
                        ActivityId = ea.ActivityId,
                        Activity = _dbContext.Activities
                            .Select(a => new ActivitySelectDto
                            {
                                Id = ea.ActivityId,
                                Name = ea.Activity.Name,
                                StartDate = ea.Activity.StartDate,
                                FinishDate = ea.Activity.FinishDate,
                                WorkOrderId = ea.Activity.WorkOrderId
                            })
                            .Single(ac => ac.Id == ea.ActivityId)
                    })
                    .ToHashSet()
            })
            .FirstAsync(ac => ac.Id == activityId);
    }

    public async Task<ActivityDeleteDto> GetForDeleteByIdAsync(string id)
    {
        return await _dbContext.Activities
            .Select(ac => new ActivityDeleteDto
            {
                Id = ac.Id,
                Name = ac.Name,
                StartDate = ac.StartDate,
                FinishDate = ac.FinishDate,
                WorkOrderId = ac.WorkOrderId,
                Employees = _dbContext.EmployeeActivity
                    .Where(ea => ea.ActivityId == id)
                    .Select(ea => new EmployeeSelectDto()
                    {
                        Id = ea.EmployeeId,
                        Email = ea.Employee.Email,
                        FirstName = ea.Employee.FirstName,
                        LastName = ea.Employee.LastName,
                        FullName = ea.Employee.FullName,
                        Role = ea.Employee.Role,
                        CurrentHourlyRate = ea.Employee.CurrentHourlyRate,
                        EmployeeSalaries = ea.Employee.Salaries
                            .Select(sal => new EmployeeSalaryDto
                            {
                                Id = sal.Id,
                                EmployeeId = ea.EmployeeId,
                                StartDate = sal.StartDate,
                                FinishDate = sal.FinishDate,
                                Salary = sal.Salary,
                            }).ToList(),
                        EmployeeActivities = ac.EmployeeActivity
                            .Select(ea => new EmployeeActivityDto
                            {
                                Id = ea.Id,
                                EmployeeId = ea.EmployeeId,
                                Employee = _dbContext.Employees
                                    .Select(em => new EmployeeSelectDto
                                    {
                                        Id = ea.EmployeeId,
                                        Email = ea.Employee.Email,
                                        FirstName = ea.Employee.FirstName,
                                        LastName = ea.Employee.LastName,
                                        FullName = ea.Employee.FullName,
                                        Role = ea.Employee.Role,
                                        CurrentHourlyRate = ea.Employee.CurrentHourlyRate,
                                        EmployeeSalaries = ea.Employee.Salaries
                                            .Select(sal => new EmployeeSalaryDto
                                            {
                                                Id = sal.Id,
                                                EmployeeId = ea.EmployeeId,
                                                StartDate = sal.StartDate,
                                                FinishDate = sal.FinishDate,
                                                Salary = sal.Salary,
                                            }).ToList()
                                    })
                                    .Single(em => em.Id == ea.EmployeeId),
                                ActivityId = ea.ActivityId,
                                Activity = _dbContext.Activities
                                    .Select(a => new ActivitySelectDto
                                    {
                                        Id = ea.ActivityId,
                                        Name = ea.Activity.Name,
                                        StartDate = ea.Activity.StartDate,
                                        FinishDate = ea.Activity.FinishDate,
                                        WorkOrderId = ea.Activity.WorkOrderId
                                    })
                                    .Single(ac => ac.Id == ea.ActivityId)
                            }).ToList()
                    })
                    .ToHashSet()
            })
            .SingleAsync(ac => ac.Id == id);
    }

    public async Task<List<ActivityViewDto>> GetActivityByEmployeeId(string employeeId, int limit = 0, int offset = 5)
    {
        return await _dbContext.Activities
            .Skip(limit)
            .Take(offset)
            .Select(ac => new ActivityViewDto
            {
                Id = ac.Id,
                Name = ac.Name,
                StartDate = ac.StartDate,
                FinishDate = ac.FinishDate,
                WorkOrderId = ac.WorkOrderId,
                WorkOrderName = _dbContext.WorkOrders
                    .Single(wo => wo.Id == ac.WorkOrderId)
                    .Name,
                ClientName = _dbContext.Clients
                    .Single(cl => _dbContext.WorkOrders
                        .Any(wo => cl.Id == wo.ClientId))
                    .Name,
                EmployeeActivity = ac.EmployeeActivity
                    .Select(ea => new EmployeeActivityDto
                    {
                        Id = ea.Id,
                        EmployeeId = ea.EmployeeId,
                        Employee = _dbContext.Employees
                            .Select(em => new EmployeeSelectDto
                            {
                                Id = ea.EmployeeId,
                                Email = ea.Employee.Email,
                                FirstName = ea.Employee.FirstName,
                                LastName = ea.Employee.LastName,
                                FullName = ea.Employee.FullName,
                                Role = ea.Employee.Role,
                                CurrentHourlyRate = ea.Employee.CurrentHourlyRate,
                                EmployeeSalaries = ea.Employee.Salaries
                                    .Select(sal => new EmployeeSalaryDto
                                    {
                                        Id = sal.Id,
                                        EmployeeId = ea.EmployeeId,
                                        StartDate = sal.StartDate,
                                        FinishDate = sal.FinishDate,
                                        Salary = sal.Salary,
                                    }).ToList()
                            })
                            .Single(em => em.Id == ea.EmployeeId),
                        ActivityId = ea.ActivityId,
                        Activity = _dbContext.Activities
                            .Select(a => new ActivitySelectDto
                            {
                                Id = ea.ActivityId,
                                Name = ea.Activity.Name,
                                StartDate = ea.Activity.StartDate,
                                FinishDate = ea.Activity.FinishDate,
                                WorkOrderId = ea.Activity.WorkOrderId
                            })
                            .Single(ac => ac.Id == ea.ActivityId)
                    }).ToHashSet()
            })
            .Where(ac => _dbContext.EmployeeActivity
                .Any(ea => ea.EmployeeId == employeeId && ac.Id == ea.ActivityId))
            .ToListAsync();
    }

    public async Task<List<ActivityViewDto>> GetActivitiesPerWorkOrderAsync(string workOrderId)
    {
        return await _dbContext.Activities
            .Where(ac => ac.WorkOrderId == workOrderId)
            .Select(ac => new ActivityViewDto
            {
                Id = ac.Id,
                Name = ac.Name,
                StartDate = ac.StartDate,
                FinishDate = ac.FinishDate,
                WorkOrderId = ac.WorkOrderId,
                ClientName = _dbContext.Clients
                    .Single(cl => _dbContext.WorkOrders
                        .Any(wo => cl.Id == wo.ClientId))
                    .Name,
                EmployeeActivity = ac.EmployeeActivity
                    .Select(ea => new EmployeeActivityDto
                    {
                        Id = ea.Id,
                        EmployeeId = ea.EmployeeId,
                        Employee = _dbContext.Employees
                            .Select(em => new EmployeeSelectDto
                            {
                                Id = ea.EmployeeId,
                                Email = ea.Employee.Email,
                                FirstName = ea.Employee.FirstName,
                                LastName = ea.Employee.LastName,
                                FullName = ea.Employee.FullName,
                                Role = ea.Employee.Role,
                                CurrentHourlyRate = ea.Employee.CurrentHourlyRate,
                                EmployeeSalaries = ea.Employee.Salaries
                                    .Select(sal => new EmployeeSalaryDto
                                    {
                                        Id = sal.Id,
                                        EmployeeId = ea.EmployeeId,
                                        StartDate = sal.StartDate,
                                        FinishDate = sal.FinishDate,
                                        Salary = sal.Salary,
                                    }).ToList()
                            })
                            .Single(em => em.Id == ea.EmployeeId),
                        ActivityId = ea.ActivityId,
                        Activity = _dbContext.Activities
                            .Select(a => new ActivitySelectDto
                            {
                                Id = ea.ActivityId,
                                Name = ea.Activity.Name,
                                StartDate = ea.Activity.StartDate,
                                FinishDate = ea.Activity.FinishDate,
                                WorkOrderId = ea.Activity.WorkOrderId
                            })
                            .Single(ac => ac.Id == ea.ActivityId)
                    }).ToHashSet()
            })
            .ToListAsync();
    }

    public async Task<List<ActivityViewDto>> GetAllAsync()
    {
        return await _dbContext.Activities
            .Select(ac => new ActivityViewDto
            {
                Id = ac.Id,
                Name = ac.Name,
                StartDate = ac.StartDate,
                FinishDate = ac.FinishDate,
                WorkOrderId = ac.WorkOrderId,
                EmployeeActivity = ac.EmployeeActivity
                    .Select(ea => new EmployeeActivityDto
                    {
                        Id = ea.Id,
                        EmployeeId = ea.EmployeeId,
                        Employee = new EmployeeSelectDto
                        {
                            Id = ea.EmployeeId,
                            Email = ea.Employee.Email,
                            FirstName = ea.Employee.FirstName,
                            LastName = ea.Employee.LastName,
                            FullName = ea.Employee.FullName,
                            Role = ea.Employee.Role,
                            CurrentHourlyRate = ea.Employee.CurrentHourlyRate,
                            EmployeeSalaries = ea.Employee.Salaries
                                .Select(sal => new EmployeeSalaryDto
                                {
                                    Id = sal.Id,
                                    EmployeeId = ea.EmployeeId,
                                    StartDate = sal.StartDate,
                                    FinishDate = sal.FinishDate,
                                    Salary = sal.Salary,
                                }).ToList()
                        },
                        ActivityId = ea.ActivityId,
                        Activity = new ActivitySelectDto
                        {
                            Id = ea.ActivityId,
                            Name = ea.Activity.Name,
                            StartDate = ea.Activity.StartDate,
                            FinishDate = ea.Activity.FinishDate,
                            WorkOrderId = ea.Activity.WorkOrderId
                        }
                    }).ToHashSet()
            })
            .ToListAsync();
    }

    public async Task<int> GetCollectionSizeByEmployeeIdAsync(string employeeId)
    {
        return (await GetActivityByEmployeeId(employeeId))
            .Count();
    }
}