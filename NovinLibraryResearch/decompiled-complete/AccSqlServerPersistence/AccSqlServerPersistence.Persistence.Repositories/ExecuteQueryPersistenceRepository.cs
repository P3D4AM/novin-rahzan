using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Core.Repository;

namespace AccSqlServerPersistence.Persistence.Repositories;

public class ExecuteQueryPersistenceRepository : IExecuteQueryRepository, IDisposable
{
	public ExecuteQueryPersistenceRepository(string connection)
	{
	}

	public async Task<List<List<string>>> executeAsync(string query, bool includeColumnNamesAsFirstRow, CancellationToken token, string tranName)
	{
		return new List<List<string>>();
	}

	public Task<List<List<string>>> executeAsync(string query, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public void Dispose()
	{
	}
}
