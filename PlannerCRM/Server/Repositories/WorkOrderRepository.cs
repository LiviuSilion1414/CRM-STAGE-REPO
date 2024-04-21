namespace PlannerCRM.Server.Repositories;

public class WorkOrderRepository
{
	private readonly AppDbContext _dbContext;
	private readonly DtoValidatorUtillity _validator;
	private readonly ILogger<DtoValidatorUtillity> _logger;

	public WorkOrderRepository(
		AppDbContext dbContext,
		DtoValidatorUtillity validator,
		Logger<DtoValidatorUtillity> logger)
	{
		_dbContext = dbContext;
		_validator = validator;
		_logger = logger;
	}

	public async Task AddAsync(WorkOrderFormDto dto)
	{
		try
		{
			var isValid = await _validator.ValidateWorkOrderAsync(dto, OperationType.ADD);

			if (isValid)
			{
				await _dbContext.WorkOrders.AddAsync(
					new WorkOrder
					{
						Id = dto.Id,
						Name = dto.Name,
						StartDate = dto.StartDate
							?? throw new NullReferenceException(ExceptionsMessages.NULL_ARG),
						FinishDate = dto.FinishDate
							?? throw new NullReferenceException(ExceptionsMessages.NULL_ARG),
						IsDeleted = false,
						IsCompleted = false,
						ClientId = dto.ClientId,
						Client = _dbContext.Clients
							.Single(cl => cl.Id == dto.ClientId)
					}
				);

				if (await _dbContext.SaveChangesAsync() == 0)
				{
					throw new DbUpdateException(ExceptionsMessages.IMPOSSIBLE_SAVE_CHANGES);
				}

				var workOrder = await _dbContext.WorkOrders
					.SingleAsync(wo => EF.Functions.ILike(wo.Name, dto.Name) && wo.ClientId == dto.ClientId);

				await SetForeignKeyToClientAsync(workOrder, OperationType.ADD);
			}
			else
			{
				throw new DbUpdateException(ExceptionsMessages.IMPOSSIBLE_ADD);
			}
		}
		catch (Exception exc)
		{
			_logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

			throw;
		}
	}

	private async Task SetForeignKeyToClientAsync(WorkOrder workOrder, OperationType operationType)
	{
		try
		{
			if (!string.IsNullOrEmpty(workOrder.Name) && await _dbContext.WorkOrders.AnyAsync(wo => wo.Id == workOrder.Id))
			{
				if (operationType == OperationType.ADD)
				{
					await _dbContext.ClientWorkOrders
						.AddAsync(
							new ClientWorkOrder
							{
								WorkOrderId = workOrder.Id,
								ClientId = workOrder.ClientId,
							}
						);
				}
				else
				{
					var clientWorkOrder = await _dbContext.ClientWorkOrders
						.SingleAsync(clwo => clwo.WorkOrderId == workOrder.Id);

					clientWorkOrder.ClientId = workOrder.ClientId;
				}

				if (await _dbContext.SaveChangesAsync() == 0)
				{
					throw new DbUpdateException(ExceptionsMessages.IMPOSSIBLE_SAVE_CHANGES);
				}
			}
			else
			{
				throw new UpdateRowSourceException(ExceptionsMessages.IMPOSSIBLE_SAVE_CHANGES);
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
		try
		{
			var workOrderDelete = await _validator.ValidateDeleteWorkOrderAsync(id);

			if (workOrderDelete is not null)
			{

				workOrderDelete.IsDeleted = true;

				if (await _dbContext.SaveChangesAsync() == 0)
				{
					throw new DbUpdateException(ExceptionsMessages.IMPOSSIBLE_SAVE_CHANGES);
				}
			}
			else
			{
				throw new DbUpdateException(ExceptionsMessages.IMPOSSIBLE_ADD);
			}
		}
		catch (Exception exc)
		{
			_logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

			throw;
		}
	}

	public async Task EditAsync(WorkOrderFormDto dto)
	{
		try
		{
			var isValid = await _validator.ValidateWorkOrderAsync(dto, OperationType.EDIT);

			if (isValid)
			{
				var model = await _dbContext.WorkOrders
					.SingleAsync(wo =>
						!wo.IsDeleted &&
						!wo.IsCompleted &&
						wo.Id == dto.Id);

				model.Id = dto.Id;
				model.Name = dto.Name;
				model.ClientId = dto.ClientId;
				model.StartDate = dto.StartDate
					?? throw new NullReferenceException(ExceptionsMessages.NULL_ARG);
				model.FinishDate = dto.FinishDate
					?? throw new NullReferenceException(ExceptionsMessages.NULL_ARG);


				if (await _dbContext.SaveChangesAsync() == 0)
				{
					throw new DbUpdateException(ExceptionsMessages.IMPOSSIBLE_SAVE_CHANGES);
				}

				await SetForeignKeyToClientAsync(model, OperationType.EDIT);
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

	public async Task<WorkOrderDeleteDto> GetForDeleteByIdAsync(string Id)
	{
		return await _dbContext.WorkOrders
			.Where(wo => !wo.IsDeleted && !wo.IsCompleted)
			.Select(wo => new WorkOrderDeleteDto
			{
				Id = wo.Id,
				Name = wo.Name,
				StartDate = wo.StartDate,
				FinishDate = wo.FinishDate,
				ClientName = _dbContext.Clients
					.Single(cl => cl.Id == wo.ClientId)
					.Name
			})
			.SingleAsync(wo => wo.Id == id);
	}

	public async Task<WorkOrderViewDto> GetForViewByIdAsync(string Id)
	{
		return await _dbContext.WorkOrders
			.Select(wo => new WorkOrderViewDto
			{
				Id = wo.Id,
				Name = wo.Name,
				StartDate = wo.StartDate,
				FinishDate = wo.FinishDate,
				IsCompleted = wo.IsCompleted,
				IsDeleted = wo.IsDeleted
			})
			.SingleAsync(wo => wo.Id == id);
	}

	public async Task<WorkOrderFormDto> GetForEditByIdAsync(string Id)
	{
		return await _dbContext.WorkOrders
			.Where(wo => !wo.IsDeleted || !wo.IsCompleted)
			.Select(wo =>
				new WorkOrderFormDto
				{
					Id = wo.Id,
					Name = wo.Name,
					StartDate = wo.StartDate,
					FinishDate = wo.FinishDate,
					ClientId = wo.ClientId,
					IsOnEdit = true,
					ClientName = _dbContext.Clients
						.Single(cl => cl.Id == wo.ClientId)
						.Name,
				}
			)
			.SingleAsync(wo => wo.Id == id);
	}

	public async Task<List<WorkOrderSelectDto>> SearchWorkOrderAsync(string workOrder)
	{
		return await _dbContext.WorkOrders
			.Where(wo => !wo.IsDeleted && !wo.IsCompleted &&
				EF.Functions.ILike(wo.Name, $"%{workOrder}%"))
			.Select(wo =>
				new WorkOrderSelectDto
				{
					Id = wo.Id,
					Name = wo.Name,
					ClientName = _dbContext.Clients
						.Single(cl => cl.Id == wo.ClientId)
						.Name
				}
			)
			.ToListAsync();
	}

	public async Task<List<WorkOrderViewDto>> GetPaginatedWorkOrdersAsync(int limit = 0, int offset = 5)
	{
		return await _dbContext.WorkOrders
			.OrderBy(wo => wo.Name)
			.Skip(limit)
			.Take(offset)
			.AsSplitQuery()
			.Select(wo => new WorkOrderViewDto
			{
				Id = wo.Id,
				Name = wo.Name,
				StartDate = wo.StartDate,
				FinishDate = wo.FinishDate,
				IsInvoiceCreated = wo.IsInvoiceCreated,
				IsCompleted = wo.IsCompleted,
				IsDeleted = wo.IsDeleted,
				ClientId = wo.ClientId,
				ClientName = _dbContext.Clients
					.Single(cl => cl.Id == wo.ClientId)
					.Name
			})
			.ToListAsync();
	}

	public async Task<int> GetWorkOrdersSizeAsync() =>
		await _dbContext.WorkOrders
			.CountAsync();
}