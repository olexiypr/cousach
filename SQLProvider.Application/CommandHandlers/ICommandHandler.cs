using SQLProvider.Application.RequestModels;
using SQLProvider.Application.ResponseModels;

namespace SQLProvider.Application.CommandHandlers;

public interface ICommandHandler
{
    Task<TableResponse> HandleCommand(CommandRequest request);
}