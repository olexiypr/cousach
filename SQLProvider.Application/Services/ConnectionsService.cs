using SQLProvider.Application.Mapper;
using SQLProvider.Application.RequestModels;
using SQLProvider.Application.ResponseModels;
using SQLProvider.Data;
using SQLProvider.Data.Entities;
using SQLProvider.Infrastructure;

namespace SQLProvider.Application.Services;

[DefaultTransientImplementation]
public class ConnectionsService : IConnectionsService
{
    private readonly IDbContext _dbContext;
    private readonly IMapper _mapper;

    public ConnectionsService(IDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<ConnectionResponse> CreateConnection(ConnectionRequest connectionRequest)
    {
        var connection = await _dbContext.CreateConnection(connectionRequest.ConnectionString, connectionRequest.DatabaseType);
        return _mapper.Map(connection);
    }

    public async Task<Connection> GetConnectionById(int connectionId)
    {
        return await _dbContext.GetConnectionById(connectionId);
    }

    public async Task<IEnumerable<ConnectionResponse>> GetAllConnections()
    {
        return (await _dbContext.GetAllConnections()).Select(c => new ConnectionResponse
        {
            Id = c.Id,
            ConnectionString = c.ConnectionString,
            DatabaseType = c.DatabaseType
        });
    }
}