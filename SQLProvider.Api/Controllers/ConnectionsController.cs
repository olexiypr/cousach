using Microsoft.AspNetCore.Mvc;
using SQLProvider.Application.RequestModels;
using SQLProvider.Application.ResponseModels;
using SQLProvider.Application.Services;

namespace SQLProvider.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ConnectionsController : ControllerBase
{
    private readonly IConnectionsService _connectionsService;

    public ConnectionsController(IConnectionsService connectionsService)
    {
        _connectionsService = connectionsService;
    }
    [HttpGet]
    public async Task<IEnumerable<ConnectionResponse>> GetAllConnections()
    {
        Console.WriteLine(HttpContext.Request.Host);
        return await _connectionsService.GetAllConnections();
    }
    [HttpPost]
    public async Task<ConnectionResponse> CreateConnection(ConnectionRequest connectionRequest)
    {
        return await _connectionsService.CreateConnection(connectionRequest);
    }
}