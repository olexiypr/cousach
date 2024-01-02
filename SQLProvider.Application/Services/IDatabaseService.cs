using SQLProvider.Application.ResponseModels;
using SQLProvider.Data.Entities;

namespace SQLProvider.Application.Services;

public interface IDatabaseService
{
    Task<IEnumerable<DatabaseResponse>> GetAllDatabases(DatabaseType databaseType);
    Task<string[]> GetAllTables(int connectionId);
}