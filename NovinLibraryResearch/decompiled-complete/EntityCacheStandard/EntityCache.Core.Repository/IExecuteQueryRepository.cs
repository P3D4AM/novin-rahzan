using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EntityCache.Core.Repository;

public interface IExecuteQueryRepository : IDisposable
{
	Task<List<List<string>>> executeAsync(string query, bool includeColumnNamesAsFirstRow, CancellationToken token, string tranName);

	Task<List<List<string>>> executeAsync(string query, CancellationToken token, string tranName);
}
