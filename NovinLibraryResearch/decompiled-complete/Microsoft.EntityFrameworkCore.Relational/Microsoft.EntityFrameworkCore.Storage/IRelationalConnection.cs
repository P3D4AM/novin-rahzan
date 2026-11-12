using System;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Storage;

public interface IRelationalConnection : IRelationalTransactionManager, IDbContextTransactionManager, IResettableService, IDisposable, IAsyncDisposable
{
	string ConnectionString { get; }

	DbConnection DbConnection { get; }

	DbContext Context { get; }

	Guid ConnectionId { get; }

	int? CommandTimeout { get; set; }

	bool IsMultipleActiveResultSetsEnabled { get; }

	new IDbContextTransaction CurrentTransaction { get; }

	SemaphoreSlim Semaphore { get; }

	bool Open(bool errorsExpected = false);

	Task<bool> OpenAsync(CancellationToken cancellationToken, bool errorsExpected = false);

	bool Close();

	Task<bool> CloseAsync();
}
