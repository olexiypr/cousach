using System.Data;
using Npgsql;

namespace SQLProvider.Postgres;

public class CommandHandler
{
    private readonly NpgsqlConnection _connection;

    public CommandHandler(string connectionString)
    {
        _connection = new NpgsqlConnection(connectionString);
    }

    public async Task<DataTable> HandleOperation(string operationText)
    {
        await _connection.OpenAsync();
        var command = new NpgsqlCommand(operationText);
        var reader = await command.ExecuteReaderAsync();
        var dataTable = new DataTable();
        dataTable.Load(reader);
        return dataTable;
    }
}