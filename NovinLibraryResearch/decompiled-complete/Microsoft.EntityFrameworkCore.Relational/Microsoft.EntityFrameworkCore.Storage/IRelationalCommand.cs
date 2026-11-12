using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.EntityFrameworkCore.Storage;

public interface IRelationalCommand
{
	string CommandText { get; }

	IReadOnlyList<IRelationalParameter> Parameters { get; }

	int ExecuteNonQuery(RelationalCommandParameterObject parameterObject);

	Task<int> ExecuteNonQueryAsync(RelationalCommandParameterObject parameterObject, CancellationToken cancellationToken = default(CancellationToken));

	object ExecuteScalar(RelationalCommandParameterObject parameterObject);

	Task<object> ExecuteScalarAsync(RelationalCommandParameterObject parameterObject, CancellationToken cancellationToken = default(CancellationToken));

	RelationalDataReader ExecuteReader(RelationalCommandParameterObject parameterObject);

	Task<RelationalDataReader> ExecuteReaderAsync(RelationalCommandParameterObject parameterObject, CancellationToken cancellationToken = default(CancellationToken));
}
