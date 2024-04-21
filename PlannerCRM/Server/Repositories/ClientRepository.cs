namespace PlannerCRM.Server.Repositories;

public class ClientRepository
{
    private readonly AppDbContext _dbContext;
    private readonly DtoValidatorUtillity _validator;
    private readonly ILogger<DtoValidatorUtillity> _logger;

    public ClientRepository(
        AppDbContext dbContext,
        DtoValidatorUtillity validator,
        Logger<DtoValidatorUtillity> logger)
    {
        _dbContext = dbContext;
        _validator = validator;
        _logger = logger;
    }

    public async Task AddClientAsync(ClientFormDto dto)
    {
        try
        {
            var isValid = await _validator.ValidateClientAsync(dto, OperationType.ADD);

            if (isValid)
            {
                await _dbContext.Clients.AddAsync(
                    new FirmClient
                    {
                        Id = dto.Id,
                        Name = dto.Name,
                        VatNumber = dto.VatNumber
                    }
                );

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

    public async Task EditClientAsync(ClientFormDto dto)
    {
        try
        {
            var isValid = await _validator.ValidateClientAsync(dto, OperationType.EDIT);

            if (isValid)
            {
                var model = await _dbContext.Clients
                    .SingleAsync(cl => cl.Id == dto.Id);

                model.Id = dto.Id;
                model.Name = dto.Name;
                model.VatNumber = dto.VatNumber;

                _dbContext.Clients.Update(model);

                if (await _dbContext.SaveChangesAsync() == 0)
                {
                    throw new DbUpdateException(ExceptionsMessages.IMPOSSIBLE_SAVE_CHANGES);
                }
            }
            else
            {
                throw new DbUpdateException(ExceptionsMessages.IMPOSSIBLE_EDIT);
            }
        }
        catch (Exception exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            throw;
        }
    }

    public async Task DeleteClientAsync(string Id)
    {
        try
        {
            var clientDelete = await _validator.ValidateDeleteClientAsync(id);

            if (clientDelete is not null)
            {
                _dbContext.Remove(clientDelete);

                if (await _dbContext.SaveChangesAsync() == 0)
                {
                    throw new DbUpdateException(ExceptionsMessages.IMPOSSIBLE_SAVE_CHANGES);
                }
            }
            else
            {
                throw new DbUpdateException(ExceptionsMessages.IMPOSSIBLE_EDIT);
            }
        }
        catch (Exception exc)
        {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            throw;
        }
    }

    public async Task<ClientViewDto> GetClientForViewByIdAsync(string clientId)
    {
        return await _dbContext.Clients
            .Where(cl => cl.Id == clientId)
            .Select(client =>
                new ClientViewDto
                {
                    Id = client.Id,
                    Name = client.Name,
                    VatNumber = client.VatNumber
                }
            )
            .SingleAsync();
    }

    public async Task<ClientFormDto> GetClientForEditByIdAsync(string Id)
    {
        return await _dbContext.Clients
            .Select(client =>
                new ClientFormDto
                {
                    Id = client.Id,
                    Name = client.Name,
                    VatNumber = client.VatNumber,
                    //WorkOrderId = client.WorkOrderId
                }
            )
            .SingleAsync(cl => cl.Id == id);
    }

    public async Task<ClientDeleteDto> GetClientForDeleteByIdAsync(string Id)
    {
        return await _dbContext.Clients
            .Select(client =>
                new ClientDeleteDto
                {
                    Id = client.Id,
                    Name = client.Name,
                    VatNumber = client.VatNumber,
                    // WorkOrderId = client.WorkOrderId
                }
            )
            .SingleAsync(cl => cl.Id == id);
    }

    public async Task<List<ClientViewDto>> GetClientsPaginatedAsync(int limit, int offset)
    {
        return await _dbContext.Clients
            .OrderBy(client => client.Id)
            .Skip(limit)
            .Take(offset)
            .Select(client =>
                new ClientViewDto
                {
                    Id = client.Id,
                    Name = client.Name,
                    VatNumber = client.VatNumber,
                    // WorkOrderId = client.WorkOrderId
                }
            )
            .ToListAsync();
    }

    public async Task<int> GetCollectionSizeAsync()
    {
        return await _dbContext.Clients
            .CountAsync();
    }

    public async Task<List<ClientViewDto>> SearchClientAsync(string clientName)
    {
        return await _dbContext.Clients
            .Select(cl => new ClientViewDto
            {
                Id = cl.Id,
                Name = cl.Name,
                VatNumber = cl.VatNumber,
                // WorkOrderId = cl.WorkOrderId
            }
            )
            .Where(cl => EF.Functions.ILike(cl.Name, $"%{clientName}%"))
            .ToListAsync();
    }

    public async Task<List<ClientViewDto>> SearchClientAsync(string clientId)
    {
        return await _dbContext.Clients
            .Select(cl => new ClientViewDto
            {
                Id = cl.Id,
                Name = cl.Name,
                VatNumber = cl.VatNumber,
                // WorkOrderId = cl.WorkOrderId
            }
            )
            .Where(cl => cl.Id == clientId)
            .ToListAsync();
    }
}