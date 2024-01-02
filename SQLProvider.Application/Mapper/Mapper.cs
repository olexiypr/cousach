using SQLProvider.Application.ResponseModels;
using SQLProvider.Data.Entities;
using SQLProvider.Infrastructure;

namespace SQLProvider.Application.Mapper;

[DefaultTransientImplementation]
public class Mapper : IMapper
{
    public ConnectionResponse Map(Connection connection)
    {
        return new ConnectionResponse
        {
            Id = connection.Id,
            ConnectionString = connection.ConnectionString,
            DatabaseType = connection.DatabaseType
        };
    }
}