using SQLProvider.Data;
using SQLProvider.Infrastructure;

namespace SQLProvider.Application.Setup;

[DefaultTransientImplementation]
public class DatabaseSetup
{
    private readonly IDbContext _dbContext;
    public DatabaseSetup(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task Setup()
    {
        await _dbContext.Setup();
    }
}