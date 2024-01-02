using SQLProvider.Data.Entities;

namespace SQLProvider.Data;

public interface IDbContext
{
    Task Setup();
    Task<Connection> CreateConnection(string connectionString, DatabaseType databaseType);
    Task<Connection> GetConnectionById(int connectionId);
    Task<IEnumerable<Connection>> GetAllConnections();
}