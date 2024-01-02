using System.Data;
using Dapper;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using Npgsql;
using SQLProvider.Application.RequestModels;
using SQLProvider.Application.ResponseModels;
using SQLProvider.Application.Services;
using SQLProvider.Data.Entities;
using SQLProvider.Infrastructure;

namespace SQLProvider.Application.CommandHandlers;

[DefaultTransientImplementation]
public class CommandHandler : ICommandHandler
{
    private readonly IConnectionsService _connectionsService;
    private readonly NpgsqlConnection _connection;

    public CommandHandler(IConnectionsService connectionsService, IConfiguration configuration)
    {
        _connectionsService = connectionsService;
        var connectionString = configuration["DataSource"];
        _connection = new NpgsqlConnection(connectionString);
    }

    public async Task<TableResponse> HandleCommand(CommandRequest request)
    {
        var connection = await _connectionsService.GetConnectionById(request.ConnectionId);
        if (connection.DatabaseType == DatabaseType.Postgres)
        {
            return await HandleCommandPostgres(connection.ConnectionString, request.Command);
        }
        return await HandleCommandMariaDb(connection.ConnectionString, request.Command);
    }

    public async Task<TableResponse> HandleCommandMariaDb(string connectionString, string commandText)
    {
        if (commandText.ToUpper().StartsWith("SELECT"))
        {
            return await SelectDataFromMariaDb(connectionString, commandText);
        }
        return await ExecuteCommandMariaDb(connectionString, commandText);
    }
    public async Task<TableResponse> HandleCommandPostgres(string connectionString, string commandText)
    {
        if (commandText.ToUpper().StartsWith("SELECT"))
        {
            return await SelectDataFromPostgres(connectionString, commandText);
        }
        return await ExecuteCommandPostgres(connectionString, commandText);
    }
    private async Task<TableResponse> ExecuteCommandMariaDb(string connectionString, string commandText)
    {
        var connection = new MySqlConnection(connectionString);
        await connection.OpenAsync();
        await connection.ExecuteAsync(commandText);
        return new TableResponse();
    }
    private async Task<TableResponse> SelectDataFromMariaDb(string connectionString, string commandText)
    {
        var connection = new MySqlConnection(connectionString);
        var tableResponse = new TableResponse();
        var result = await connection.QueryAsync(commandText);
        foreach (IDictionary<string, object> dict in result)
        {
            tableResponse.Columns = dict.Keys;
            tableResponse.Values.Add(dict.ToDictionary(pair => pair.Key, pair => pair.Value.ToString()));
        }
        return tableResponse;
    }
    private async Task<TableResponse> ExecuteCommandPostgres(string connectionString, string commandText)
    {
        var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync();
        await connection.ExecuteAsync(commandText);
        return new TableResponse();
    }
    private async Task<TableResponse> SelectDataFromPostgres(string connectionString, string commandText)
    {
        var connection = new NpgsqlConnection(connectionString);
        var tableResponse = new TableResponse();
        var result = await connection.QueryAsync(commandText);
        foreach (IDictionary<string, object> dict in result)
        {
            tableResponse.Columns = dict.Keys;
            tableResponse.Values.Add(dict.ToDictionary(pair => pair.Key, pair => pair.Value.ToString()));
        }
        return tableResponse;
    }
}