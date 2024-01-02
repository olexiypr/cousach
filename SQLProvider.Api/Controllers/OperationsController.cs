using Microsoft.AspNetCore.Mvc;
using SQLProvider.Application.CommandHandlers;
using SQLProvider.Application.RequestModels;
using SQLProvider.Application.ResponseModels;

namespace SQLProvider.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OperationsController : ControllerBase
{
    private readonly ICommandHandler _commandHandler;

    public OperationsController(ICommandHandler commandHandler)
    {
        _commandHandler = commandHandler;
    }
    [HttpPost]
    public async Task<TableResponse> ExecuteCommand(CommandRequest commandRequest)
    {
        return await _commandHandler.HandleCommand(commandRequest);
    }
}