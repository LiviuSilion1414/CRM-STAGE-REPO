namespace PlannerCRM.Server.Repositories;

public class WorkTimeRecordRepository
{
    private readonly AppDbContext _dbContext;
    private readonly DtoValidatorUtillity _validator;
    private readonly ILogger<DtoValidatorUtillity> _logger;

    public WorkTimeRecordRepository(
        AppDbContext db,
        DtoValidatorUtillity validator,
        Logger<DtoValidatorUtillity> logger)
    {
        _dbContext = db;
        _validator = validator;
        _logger = logger;
    }

    public async Task AddAsync(WorkTimeRecordFormDto dto)
    {
        try
        {
            var isValid = _validator.ValidateWorkTime(dto);

            if (isValid)
            {
                await _dbContext.WorkTimeRecords.AddAsync(
                    new WorkTimeRecord
                    {
                        Id = dto.Id,
                        Date = dto.Date,
                        Hours = dto.Hours
                            ?? throw new ArgumentNullException(nameof(dto.Hours), ExceptionsMessages.NULL_ARG),
                        TotalPrice = dto.TotalPrice + dto.Hours
                            ?? throw new ArgumentNullException(nameof(dto.Hours), ExceptionsMessages.NULL_ARG),
                        ActivityId = dto.ActivityId,
                        EmployeeId = dto.EmployeeId,
                        Employee = _dbContext.Employees
                            .Where(em => !em.IsDeleted && !em.IsArchived)
                            .Single(e => e.Id == dto.EmployeeId),
                        WorkOrderId = await _dbContext.WorkOrders
                            .AnyAsync(wo => !wo.IsDeleted && !wo.IsCompleted)
                                ? dto.WorkOrderId
                                : throw new InvalidOperationException(ExceptionsMessages.IMPOSSIBLE_ADD)
                    }
                );

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

    public async Task DeleteAsync(string Id)
    {
        var workTimeRecordDelete = await _dbContext.WorkTimeRecords
            .SingleAsync(wtr => wtr.Id == id)
                ?? throw new KeyNotFoundException(ExceptionsMessages.OBJECT_NOT_FOUND);

        _dbContext.WorkTimeRecords.Remove(workTimeRecordDelete);

        if (await _dbContext.SaveChangesAsync() == 0)
        {
            throw new DbUpdateException(ExceptionsMessages.IMPOSSIBLE_SAVE_CHANGES);
        }
    }

    public async Task EditAsync(WorkTimeRecordFormDto dto)
    {
        try
        {
            var isValid = _validator.ValidateWorkTime(dto);

            if (isValid)
            {
                var model = await _dbContext.WorkTimeRecords
                    .SingleAsync(wtr => wtr.Id == dto.Id);

                model.Id = dto.Id;
                model.Date = dto.Date;
                model.Hours = dto.Hours
                    ?? throw new ArgumentNullException(nameof(dto.Hours), ExceptionsMessages.IMPOSSIBLE_EDIT);
                model.TotalPrice = dto.TotalPrice;
                model.ActivityId = dto.ActivityId;
                model.WorkOrderId = dto.WorkOrderId;
                model.EmployeeId = dto.EmployeeId;
                model.Employee = await _dbContext.Employees
                    .SingleAsync(em => !em.IsDeleted && !em.IsArchived && em.Id == dto.EmployeeId);
                _dbContext.Update(model);

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

    public async Task<WorkTimeRecordViewDto> GetAsync(int workOrderId, int activityId, string employeeId)
    {
        var hasElements = await _dbContext.WorkTimeRecords
            .AnyAsync(wtr =>
                wtr.ActivityId == activityId &&
                wtr.EmployeeId == employeeId);

        return hasElements
            ? await _dbContext.WorkTimeRecords
                .Select(wtr => new WorkTimeRecordViewDto
                {
                    Id = wtr.Id,
                    Date = wtr.Date,
                    Hours = _dbContext.WorkTimeRecords
                        .Where(wtr =>
                            wtr.WorkOrderId == workOrderId &&
                            wtr.ActivityId == activityId &&
                            wtr.EmployeeId == employeeId)
                        .Distinct()
                        .Sum(wtrSum => wtrSum.Hours),
                    TotalPrice = wtr.TotalPrice,
                    ActivityId = wtr.ActivityId,
                    WorkOrderId = wtr.WorkOrderId,
                    EmployeeId = wtr.EmployeeId
                })
                .OrderByDescending(wtr => wtr.Hours)
                .FirstAsync(wtr =>
                    wtr.WorkOrderId == workOrderId &&
                    wtr.ActivityId == activityId &&
                    wtr.EmployeeId == employeeId)
            : new();
    }

    public async Task<List<WorkTimeRecordViewDto>> GetPaginatedWorkTimeRecordsAsync(int limit, int offset)
    {
        return await _dbContext.WorkTimeRecords
            .OrderBy(wtr => wtr.Id)
            .Skip(limit)
            .Take(offset)
            .Select(wtr => new WorkTimeRecordViewDto
            {
                Id = wtr.Id,
                Date = wtr.Date,
                Hours = wtr.Hours,
                TotalPrice = wtr.TotalPrice,
                ActivityId = wtr.ActivityId,
                EmployeeId = wtr.EmployeeId
            })
            .ToListAsync();
    }

    public async Task<WorkTimeRecordViewDto> GetAllWorkTimeRecordsByEmployeeIdAsync(string employeeId)
    {
        return await _dbContext.WorkTimeRecords
            .Select(wtr =>
                new WorkTimeRecordViewDto
                {
                    Id = wtr.Id,
                    Date = wtr.Date,
                    Hours = _dbContext.WorkTimeRecords
                        .Sum(wtrSum => wtrSum.Hours),
                    TotalPrice = wtr.TotalPrice,
                    ActivityId = wtr.ActivityId,
                    EmployeeId = wtr.EmployeeId
                })
            .SingleAsync(wtr => wtr.EmployeeId == employeeId);
    }
}