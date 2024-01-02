namespace SQLProvider.Data.Entities;

public class Connection
{
    public int Id { get; set; }
    public string ConnectionString { get; set; }
    public DatabaseType DatabaseType { get; set; }
}