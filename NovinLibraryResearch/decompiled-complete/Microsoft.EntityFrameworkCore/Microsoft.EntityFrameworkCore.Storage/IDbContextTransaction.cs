using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.EntityFrameworkCore.Storage;

public interface IDbContextTransaction : IDisposable, IAsyncDisposable
{
	Guid TransactionId { get; }

	void Commit();

	void Rollback();

	Task CommitAsync(CancellationToken cancellationToken = default(CancellationToken));

	Task RollbackAsync(CancellationToken cancellationToken = default(CancellationToken));
}
