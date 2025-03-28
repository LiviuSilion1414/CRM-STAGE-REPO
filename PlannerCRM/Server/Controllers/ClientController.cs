namespace PlannerCRM.Server.Controllers;

[Authorize]
[ApiController]
[Route(ApiUrl.CLIENT_CONTROLLER)]
public class ClientController(FirmClientRepository repo) : ControllerBase
{
    private readonly FirmClientRepository _repo = repo;

    [HttpPost]
    [Route(ApiUrl.CLIENT_INSERT)]
    public async Task<ResultDto> Insert(FirmClientDto dto)
    {
        try
        {
            await _repo.Insert(dto);
            return new ResultDto()
            {
                id = null,
                data = null,
                hasCompleted = true,
                message = "Operation completed",
                messageType = MessageType.Success,
                statusCode = HttpStatusCode.OK
            };
        } 
        catch 
        {
            return new ResultDto()
            {
                id = null,
                data = null,
                hasCompleted = false,
                message = "Operation failed",
                messageType = MessageType.Error,
                statusCode = HttpStatusCode.NotFound
            };
        }
    }

    [HttpPut]
    [Route(ApiUrl.CLIENT_UPDATE)]
    public async Task<ResultDto> Update(FirmClientDto dto)
    {
        try
        {
            await _repo.Update(dto);
            return new ResultDto()
            {
                id = null,
                data = null,
                hasCompleted = true,
                message = "Operation completed",
                messageType = MessageType.Success,
                statusCode = HttpStatusCode.OK
            };
        } 
        catch 
        {
            return new ResultDto()
            {
                id = null,
                data = null,
                hasCompleted = false,
                message = "Operation failed",
                messageType = MessageType.Error,
                statusCode = HttpStatusCode.NotFound
            };
        }
    }

    [HttpPost]
    [Route(ApiUrl.CLIENT_DELETE)]
    public async Task<ResultDto> Delete([FromBody] FirmClientFilterDto filter)
    {
        try
        {
            await _repo.Delete(filter);
            return new ResultDto()
            {
                id = null,
                data = null,
                hasCompleted = true,
                message = "Operation completed",
                messageType = MessageType.Success,
                statusCode = HttpStatusCode.OK
            };
        } 
        catch 
        {
            return new ResultDto()
            {
                id = null,
                data = null,
                hasCompleted = false,
                message = "Operation failed",
                messageType = MessageType.Error,
                statusCode = HttpStatusCode.NotFound
            };
        }
    }

    [HttpPost]
    [Route(ApiUrl.CLIENT_GET)]
    public async Task<ResultDto> Get([FromBody] FirmClientFilterDto filter)
    {
        try
        {
            var client = await _repo.Get(filter);
            return new ResultDto()
            {
                id = null,
                data = client,
                hasCompleted = true,
                message = "Operation completed",
                messageType = MessageType.Success,
                statusCode = HttpStatusCode.OK
            };
        } 
        catch 
        {
            return new ResultDto()
            {
                id = null,
                data = null,
                hasCompleted = false,
                message = "Operation failed",
                messageType = MessageType.Error,
                statusCode = HttpStatusCode.NotFound
            };
        }
    }

    [HttpPost]
    [Route(ApiUrl.CLIENT_LIST)]
    public async Task<ResultDto> List([FromBody] FirmClientFilterDto filter)
    {
        try
        {
            var entities = await _repo.List(filter);
            return new ResultDto()
            {
                id = null,
                data = entities,
                hasCompleted = true,
                message = "Operation completed",
                messageType = MessageType.Success,
                statusCode = HttpStatusCode.OK
            };
        } 
        catch 
        {
            return new ResultDto()
            {
                id = null,
                data = null,
                hasCompleted = false,
                message = "Operation failed",
                messageType = MessageType.Error,
                statusCode = HttpStatusCode.NotFound
            };
        }
    }

    [HttpPost]
    [Route(ApiUrl.CLIENT_SEARCH)]
    public async Task<ResultDto> SearchClientByName([FromBody] FirmClientFilterDto filter)
    {
        try
        {
            var client = await _repo.Search(filter);
            return new ResultDto()
            {
                id = null,
                data = client,
                hasCompleted = true,
                message = "Operation completed",
                messageType = MessageType.Success,
                statusCode = HttpStatusCode.OK
            };
        } 
        catch 
        {
            return new ResultDto()
            {
                id = null,
                data = null,
                hasCompleted = false,
                message = "Operation failed",
                messageType = MessageType.Error,
                statusCode = HttpStatusCode.NotFound
            };
        }
    }

    [HttpPost]
    [Route(ApiUrl.CLIENT_FIND_ASSOCIATED_WORKORDERS_BY_CLIENTID)]
    public async Task<ResultDto> FindAssociatedWorkOrdersByClientId([FromBody] FirmClientFilterDto filter)
    {
        try
        {
            var workOrders = await _repo.FindAssociatedWorkOrdersByClientId(filter);
            return new ResultDto()
            {
                id = null,
                data = workOrders,
                hasCompleted = true,
                message = "Operation completed",
                messageType = MessageType.Success,
                statusCode = HttpStatusCode.OK
            };
        } 
        catch 
        {
            return new ResultDto()
            {
                id = null,
                data = null,
                hasCompleted = false,
                message = "Operation failed",
                messageType = MessageType.Error,
                statusCode = HttpStatusCode.NotFound
            };
        }
    }
}