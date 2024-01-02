using SQLProvider.Data.Entities;

namespace SQLProvider.Application.ResponseModels;

public class ConnectionResponse
{
    public int Id { get; set; }
    public string ConnectionString { get; set; }
    public DatabaseType DatabaseType { get; set; }
}