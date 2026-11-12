using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Diagnostics;

public interface IDbTransactionInterceptor : IInterceptor
{
	InterceptionResult<DbTransaction> TransactionStarting([JetBrains.Annotations.NotNull] DbConnection connection, [JetBrains.Annotations.NotNull] TransactionStartingEventData eventData, InterceptionResult<DbTransaction> result);

	DbTransaction TransactionStarted([JetBrains.Annotations.NotNull] DbConnection connection, [JetBrains.Annotations.NotNull] TransactionEndEventData eventData, [JetBrains.Annotations.CanBeNull] DbTransaction result);

	Task<InterceptionResult<DbTransaction>> TransactionStartingAsync([JetBrains.Annotations.NotNull] DbConnection connection, [JetBrains.Annotations.NotNull] TransactionStartingEventData eventData, InterceptionResult<DbTransaction> result, CancellationToken cancellationToken = default(CancellationToken));

	Task<DbTransaction> TransactionStartedAsync([JetBrains.Annotations.NotNull] DbConnection connection, [JetBrains.Annotations.NotNull] TransactionEndEventData eventData, [JetBrains.Annotations.CanBeNull] DbTransaction result, CancellationToken cancellationToken = default(CancellationToken));

	DbTransaction TransactionUsed([JetBrains.Annotations.NotNull] DbConnection connection, [JetBrains.Annotations.NotNull] TransactionEventData eventData, [JetBrains.Annotations.CanBeNull] DbTransaction result);

	Task<DbTransaction> TransactionUsedAsync([JetBrains.Annotations.NotNull] DbConnection connection, [JetBrains.Annotations.NotNull] TransactionEventData eventData, [JetBrains.Annotations.CanBeNull] DbTransaction result, CancellationToken cancellationToken = default(CancellationToken));

	InterceptionResult TransactionCommitting([JetBrains.Annotations.NotNull] DbTransaction transaction, [JetBrains.Annotations.NotNull] TransactionEventData eventData, InterceptionResult result);

	void TransactionCommitted([JetBrains.Annotations.NotNull] DbTransaction transaction, [JetBrains.Annotations.NotNull] TransactionEndEventData eventData);

	Task<InterceptionResult> TransactionCommittingAsync([JetBrains.Annotations.NotNull] DbTransaction transaction, [JetBrains.Annotations.NotNull] TransactionEventData eventData, InterceptionResult result, CancellationToken cancellationToken = default(CancellationToken));

	Task TransactionCommittedAsync([JetBrains.Annotations.NotNull] DbTransaction transaction, [JetBrains.Annotations.NotNull] TransactionEndEventData eventData, CancellationToken cancellationToken = default(CancellationToken));

	InterceptionResult TransactionRollingBack([JetBrains.Annotations.NotNull] DbTransaction transaction, [JetBrains.Annotations.NotNull] TransactionEventData eventData, InterceptionResult result);

	void TransactionRolledBack([JetBrains.Annotations.NotNull] DbTransaction transaction, [JetBrains.Annotations.NotNull] TransactionEndEventData eventData);

	Task<InterceptionResult> TransactionRollingBackAsync([JetBrains.Annotations.NotNull] DbTransaction transaction, [JetBrains.Annotations.NotNull] TransactionEventData eventData, InterceptionResult result, CancellationToken cancellationToken = default(CancellationToken));

	Task TransactionRolledBackAsync([JetBrains.Annotations.NotNull] DbTransaction transaction, [JetBrains.Annotations.NotNull] TransactionEndEventData eventData, CancellationToken cancellationToken = default(CancellationToken));

	void TransactionFailed([JetBrains.Annotations.NotNull] DbTransaction transaction, [JetBrains.Annotations.NotNull] TransactionErrorEventData eventData);

	Task TransactionFailedAsync([JetBrains.Annotations.NotNull] DbTransaction transaction, [JetBrains.Annotations.NotNull] TransactionErrorEventData eventData, CancellationToken cancellationToken = default(CancellationToken));
}
