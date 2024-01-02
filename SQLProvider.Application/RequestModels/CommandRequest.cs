namespace SQLProvider.Application.RequestModels;

public class CommandRequest
{
    public string Command { get; set; }
    public int ConnectionId { get; set; }
}