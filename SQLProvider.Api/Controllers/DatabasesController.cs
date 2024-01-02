using Microsoft.AspNetCore.Mvc;
using SQLProvider.Application.ResponseModels;
using SQLProvider.Application.Services;
using SQLProvider.Data.Entities;

namespace SQLProvider.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DatabasesController : ControllerBase
{
    private readonly IDatabaseService _databaseService;

    public DatabasesController(IDatabaseService databaseService)
    {
        _databaseService = databaseService;
    }
    [HttpGet]
    public async Task<IEnumerable<DatabaseResponse>> GetAllDatabases(DatabaseType databaseType)
    {
        return await _databaseService.GetAllDatabases(databaseType);
    }

    [HttpGet("{connectionId:int}")]
    public async Task<string[]> GetAllTablesInDatabase(int connectionId)
    {
        return await _databaseService.GetAllTables(connectionId);
    }
}