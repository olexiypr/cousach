using Dapper;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using Npgsql;
using SQLProvider.Data.Entities;
using SQLProvider.Data.Exceptions;
using SQLProvider.Infrastructure;

namespace SQLProvider.Data;

[DefaultTransientImplementation]
public class DbContext : IDbContext
{
    private readonly NpgsqlConnection _npgsqlConnection;
    private readonly IConfiguration _configuration;
    
    public DbContext(IConfiguration configuration)
    {
        _configuration = configuration;
        var connectionString = configuration["DataSource"];
        _npgsqlConnection = new NpgsqlConnection(connectionString);
    }

    public async Task Setup()
    {
        var connection = new NpgsqlConnection(_configuration["DataSource"].Replace("sql_provider", "postgres"));
        await connection.ExecuteAsync("create database sql_provider");
        connection = new NpgsqlConnection(_configuration["DataSource"]);
        await connection.ExecuteAsync(@"CREATE TABLE Connections
                                                            (
                                                                id serial primary key,
                                                                connectionString varchar,
                                                                databaseType integer
                                                            )");
    }
    public async Task<Connection> CreateConnection(string connectionString, DatabaseType databaseType)
    {
        if (databaseType == DatabaseType.Postgres)
        {
            try
            {
                var npgSqlConnection = new NpgsqlConnection(connectionString);
                await npgSqlConnection.QueryAsync<string>(
                    "SELECT table_name FROM information_schema.tables WHERE table_schema='public';");
            }
            catch (Exception e)
            {
                throw new InvalidConnectionStringException();
            }
        }
        else
        {
            try
            {
                var mySqlConnection = new MySqlConnection(connectionString);
                await mySqlConnection.QueryAsync<string>("SHOW TABLES;");
            }
            catch (Exception e)
            {
                throw new InvalidConnectionStringException();
            }
            
        }
        var id = await _npgsqlConnection.QueryFirstOrDefaultAsync<int>(@"insert into Connections (connectionstring, databasetype) values (@cs, @dbType) returning id", 
        new {cs = connectionString, dbType = databaseType});
        return await GetConnectionById(id);
    }

    public async Task<Connection> GetConnectionById(int connectionId)
    {
        var connection = await _npgsqlConnection.QueryFirstOrDefaultAsync<Connection>(@"select * from Connections where id = @id",
            new { id = connectionId });
        if (connection is null)
        {
            throw new ConnectionNotFoundException(connectionId);
        }
        return connection;
    }

    public async Task<IEnumerable<Connection>> GetAllConnections()
    {
        return await _npgsqlConnection.QueryAsync<Connection>(@"select * from Connections");
    }
}