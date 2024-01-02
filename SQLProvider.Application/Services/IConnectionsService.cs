using SQLProvider.Application.RequestModels;
using SQLProvider.Application.ResponseModels;
using SQLProvider.Data.Entities;

namespace SQLProvider.Application.Services;

public interface IConnectionsService
{
    Task<ConnectionResponse> CreateConnection(ConnectionRequest connectionRequest);
    Task<Connection> GetConnectionById(int connectionId);
    Task<IEnumerable<ConnectionResponse>> GetAllConnections();
}