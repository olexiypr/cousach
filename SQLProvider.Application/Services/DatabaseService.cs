using Dapper;
using MySqlConnector;
using Npgsql;
using SQLProvider.Application.ResponseModels;
using SQLProvider.Data;
using SQLProvider.Data.Entities;
using SQLProvider.Infrastructure;

namespace SQLProvider.Application.Services;

[DefaultTransientImplementation]
public class DatabaseService : IDatabaseService
{
    private readonly IDbContext _dbContext;

    public DatabaseService(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<DatabaseResponse>> GetAllDatabases(DatabaseType databaseType)
    {
        IEnumerable<string> dbNames = new List<string>();
        if (databaseType == DatabaseType.Postgres)
        {
            var npgsqlConnection = new NpgsqlConnection("Host=postgresdb2;Username=admin1;Password=password1;Database=postgres;Port=4000");
            dbNames = await npgsqlConnection.QueryAsync<string>(@"SELECT datname FROM pg_database where datacl is null");
        }
        else
        {
            var mySqlConnection = new MySqlConnection("server=mariadb;port=3306;user=root;password=password1;");
            dbNames =  await mySqlConnection.QueryAsync<string>(@"show databases;");
        }
        
        if (databaseType == DatabaseType.Postgres)
        {
            return dbNames.Select(d => new DatabaseResponse
            {
                Name = d,
                ConnectionString = $"Host=postgresdb2;Username=admin1;Password=password1;Database={d};Port=4000"
            });
        }

        return dbNames.Where(d => d != "information_schema" && d != "performance_schema" && d != "sys").Select(d => new DatabaseResponse
        {
            Name = d,
            ConnectionString = $"server=mariadb;port=3306;database={d};user=root;password=password1;"
        });
    }

    public async Task<string[]> GetAllTables(int connectionId)
    {
        var connection = await _dbContext.GetConnectionById(connectionId);
        IEnumerable<string> tables = new List<string>();
        if (connection.DatabaseType == DatabaseType.Postgres)
        {
            var npgSqlConnection = new NpgsqlConnection(connection.ConnectionString);
            tables = await npgSqlConnection.QueryAsync<string>(
                    "SELECT table_name FROM information_schema.tables WHERE table_schema='public';");
        }
        else
        {
            var mySqlConnection = new MySqlConnection(connection.ConnectionString);
            tables = await mySqlConnection.QueryAsync<string>("SHOW TABLES;");
        }
        return tables.ToArray();
    }
}