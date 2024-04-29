using System.Data;

namespace Bookify.Application.Abstractions.Data;

public interface ISqlConnectionFactory
{
    Task<IDbConnection> CreateConnectionAsync();
}