using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.EntityFrameworkCore.Diagnostics;

public abstract class DbTransactionInterceptor : IDbTransactionInterceptor, IInterceptor
{
	public virtual InterceptionResult<DbTransaction> TransactionStarting(DbConnection connection, TransactionStartingEventData eventData, InterceptionResult<DbTransaction> result)
	{
		return result;
	}

	public virtual DbTransaction TransactionStarted(DbConnection connection, TransactionEndEventData eventData, DbTransaction result)
	{
		return result;
	}

	public virtual Task<InterceptionResult<DbTransaction>> TransactionStartingAsync(DbConnection connection, TransactionStartingEventData eventData, InterceptionResult<DbTransaction> result, CancellationToken cancellationToken = default(CancellationToken))
	{
		return Task.FromResult(result);
	}

	public virtual Task<DbTransaction> TransactionStartedAsync(DbConnection connection, TransactionEndEventData eventData, DbTransaction result, CancellationToken cancellationToken = default(CancellationToken))
	{
		return Task.FromResult(result);
	}

	public virtual DbTransaction TransactionUsed(DbConnection connection, TransactionEventData eventData, DbTransaction result)
	{
		return result;
	}

	public virtual Task<DbTransaction> TransactionUsedAsync(DbConnection connection, TransactionEventData eventData, DbTransaction result, CancellationToken cancellationToken = default(CancellationToken))
	{
		return Task.FromResult(result);
	}

	public virtual InterceptionResult TransactionCommitting(DbTransaction transaction, TransactionEventData eventData, InterceptionResult result)
	{
		return result;
	}

	public virtual void TransactionCommitted(DbTransaction transaction, TransactionEndEventData eventData)
	{
	}

	public virtual Task<InterceptionResult> TransactionCommittingAsync(DbTransaction transaction, TransactionEventData eventData, InterceptionResult result, CancellationToken cancellationToken = default(CancellationToken))
	{
		return Task.FromResult(result);
	}

	public virtual Task TransactionCommittedAsync(DbTransaction transaction, TransactionEndEventData eventData, CancellationToken cancellationToken = default(CancellationToken))
	{
		return Task.CompletedTask;
	}

	public virtual InterceptionResult TransactionRollingBack(DbTransaction transaction, TransactionEventData eventData, InterceptionResult result)
	{
		return result;
	}

	public virtual void TransactionRolledBack(DbTransaction transaction, TransactionEndEventData eventData)
	{
	}

	public virtual Task<InterceptionResult> TransactionRollingBackAsync(DbTransaction transaction, TransactionEventData eventData, InterceptionResult result, CancellationToken cancellationToken = default(CancellationToken))
	{
		return Task.FromResult(result);
	}

	public virtual Task TransactionRolledBackAsync(DbTransaction transaction, TransactionEndEventData eventData, CancellationToken cancellationToken = default(CancellationToken))
	{
		return Task.CompletedTask;
	}

	public virtual void TransactionFailed(DbTransaction transaction, TransactionErrorEventData eventData)
	{
	}

	public virtual Task TransactionFailedAsync(DbTransaction transaction, TransactionErrorEventData eventData, CancellationToken cancellationToken = default(CancellationToken))
	{
		return Task.CompletedTask;
	}
}
