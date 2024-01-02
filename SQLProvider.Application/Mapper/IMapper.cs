using SQLProvider.Application.ResponseModels;
using SQLProvider.Data.Entities;

namespace SQLProvider.Application.Mapper;

public interface IMapper
{
    ConnectionResponse Map(Connection connection);
}