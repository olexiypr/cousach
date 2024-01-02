using SQLProvider.Data.Entities;

namespace SQLProvider.Application.RequestModels;

public class ConnectionRequest
{
    public string ConnectionString { get; set; }
    public DatabaseType DatabaseType { get; set; }
}